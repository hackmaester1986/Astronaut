using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Controllers;
using Stargate.Services;
using StargateAPI.Services;
using System.Threading.Tasks;
using Stargate.Repositories;
using StargateAPI.Business.Data;

namespace UnitTests.Controllers
{
    public class AstronautDetailControllerTests
    {
        private readonly Mock<IAstronautDetailService> _mockService;
        private readonly AstronautDetailController _controller;
        private readonly Mock<IProcessLogRepository> _mockLogRepo;

        public AstronautDetailControllerTests()
        {
            _mockService = new Mock<IAstronautDetailService>();
            _mockLogRepo = new Mock<IProcessLogRepository>();
            _controller = new AstronautDetailController(_mockService.Object, _mockLogRepo.Object);
        }

        [Fact]
        public async Task CreateAstronautDetail_ReturnsBadRequest_WhenRequestIsNull()
        {
            // Act
            var result = await _controller.CreateAstronautDetail(null);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid request data.", badRequest.Value);
        }

        [Fact]
        public async Task CreateAstronautDetail_ReturnsBadRequest_WhenDataIsInvalid()
        {
            var invalidRequest = new CreateAstronautDetailDto { Name = "", Rank = "", Title = "" };

            var result = await _controller.CreateAstronautDetail(invalidRequest);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid request data.", badRequest.Value);
        }

        [Fact]
        public async Task CreateAstronautDetail_ReturnsBadRequest_WhenServiceFails()
        {
            var request = new CreateAstronautDetailDto { Name = "John", Rank = "Captain", Title = "Pilot",CareerStartDate = DateTime.UtcNow };
            _mockService.Setup(s => s.CreateAstronautDetail(request.Name, request.Rank, request.Title,request.CareerStartDate))
                        .ReturnsAsync(false);

            var result = await _controller.CreateAstronautDetail(request);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to add duty.", badRequest.Value);
        }

        [Fact]
        public async Task CreateAstronautDetail_ReturnsOk_WhenServiceSucceeds()
        {
            var request = new CreateAstronautDetailDto { Name = "John", Rank = "Captain", Title = "Pilot",CareerStartDate = DateTime.UtcNow };
            _mockService.Setup(s => s.CreateAstronautDetail(request.Name, request.Rank, request.Title,request.CareerStartDate))
                        .ReturnsAsync(true);

            var result = await _controller.CreateAstronautDetail(request);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateAstronautDetails_ReturnsBadRequest_WhenRequestIsNull()
        {
            var result = await _controller.UpdateAstronautDetails(null);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid request data.", badRequest.Value);
        }

        [Fact]
        public async Task UpdateAstronautDetails_ReturnsBadRequest_WhenNameIsMissing()
        {
            var request = new AstronautDetailRequestDto { Name = "" };

            var result = await _controller.UpdateAstronautDetails(request);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid request data.", badRequest.Value);
        }

        [Fact]
        public async Task UpdateAstronautDetails_ReturnsBadRequest_WhenServiceFails()
        {
            var request = new AstronautDetailRequestDto { Name = "John", CurrentRank = "Captain", CurrentDutyTitle = "Pilot" };
            _mockService.Setup(s => s.UpdateAstronautDetailAsync(request))
                        .ReturnsAsync(false);

            var result = await _controller.UpdateAstronautDetails(request);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to update detail.", badRequest.Value);
        }

        [Fact]
        public async Task UpdateAstronautDetails_ReturnsOk_WhenServiceSucceeds()
        {
            var request = new AstronautDetailRequestDto { Name = "John", CurrentRank = "Captain", CurrentDutyTitle = "Pilot" };
            _mockService.Setup(s => s.UpdateAstronautDetailAsync(request))
                        .ReturnsAsync(true);

            var result = await _controller.UpdateAstronautDetails(request);

            Assert.IsType<OkResult>(result);
        }
        
        [Fact]
        public async Task CreateAstronautDetail_LogsInfo_OnSuccess()
        {
            var dto = new CreateAstronautDetailDto { Name = "Daniel", Rank = "Doctor", Title = "Scholar",CareerStartDate = DateTime.UtcNow };
            _mockService.Setup(s => s.CreateAstronautDetail(dto.Name, dto.Rank, dto.Title,dto.CareerStartDate)).ReturnsAsync(true);

            var result = await _controller.CreateAstronautDetail(dto);

            Assert.IsType<OkResult>(result);
            _mockLogRepo.Verify(r => r.AddLogAsync(It.Is<ProcessLog>(
                log => log.Level == "INFO" && log.Message.Contains("Created Daniel astronaut detail")
            )), Times.Once);
        }

        [Fact]
        public async Task CreateAstronautDetail_LogsError_OnFailure()
        {
            var dto = new CreateAstronautDetailDto { Name = "Daniel", Rank = "Doctor", Title = "Scholar",CareerStartDate = DateTime.UtcNow };
            _mockService.Setup(s => s.CreateAstronautDetail(dto.Name, dto.Rank, dto.Title,dto.CareerStartDate)).ReturnsAsync(false);

            var result = await _controller.CreateAstronautDetail(dto);

            Assert.IsType<BadRequestObjectResult>(result);
            _mockLogRepo.Verify(r => r.AddLogAsync(It.Is<ProcessLog>(
                log => log.Level == "ERROR" && log.Message.Contains("Failed to create Daniel astronaut detail")
            )), Times.Once);
        }
    }


}
