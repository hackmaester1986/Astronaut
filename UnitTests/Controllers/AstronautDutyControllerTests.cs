using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Controllers;
using Stargate.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using StargateAPI.Business.Data;
using Stargate.Repositories;

namespace UnitTests.Controllers
{
    public class AstronautDutyControllerTests
    {
        private readonly Mock<IAstronautDutyService> _mockService;
        private readonly AstronautDutyController _controller;
        private readonly Mock<IProcessLogRepository> _mockLogRepo;

        public AstronautDutyControllerTests()
        {
            _mockService = new Mock<IAstronautDutyService>();
            _mockLogRepo = new Mock<IProcessLogRepository>();

            _controller = new AstronautDutyController(_mockService.Object, _mockLogRepo.Object);
        }

        // --- GetAstronautDutiesByName ---

        [Fact]
        public async Task GetAstronautDutiesByName_ReturnsOk_WithDuties()
        {
            // Arrange
            var name = "Jack";
            var duties = new List<AstronautDuty>
            {
                new AstronautDuty { Id = 1, PersonId = 1, Rank = "Colonel", DutyTitle = "Mission Leader", DutyStartDate = DateTime.UtcNow },
                new AstronautDuty { Id = 2, PersonId = 1, Rank = "Major", DutyTitle = "Trainer", DutyStartDate = DateTime.UtcNow }
            };

            _mockService.Setup(s => s.GetDutiesByAstronautNameAsync(name))
                        .ReturnsAsync(duties);

            // Act
            var result = await _controller.GetAstronautDutiesByName(name);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<AstronautDuty>>(okResult.Value);

            Assert.Collection(returnValue,
                d => Assert.Equal("Mission Leader", d.DutyTitle),
                d => Assert.Equal("Trainer", d.DutyTitle));
        }

        // --- CreateAstronautDuty ---

        [Fact]
        public async Task CreateAstronautDuty_ReturnsBadRequest_WhenRequestIsNull()
        {
            // Act
            var result = await _controller.CreateAstronautDuty(null);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid request data.", badRequest.Value);
        }

        [Fact]
        public async Task CreateAstronautDuty_ReturnsBadRequest_WhenDataIsInvalid()
        {
            var request = new CreateAstronautDutyDto { Name = "", DutyDescription = "" };

            var result = await _controller.CreateAstronautDuty(request);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid request data.", badRequest.Value);
        }

        [Fact]
        public async Task CreateAstronautDuty_ReturnsBadRequest_WhenServiceFails()
        {
            var request = new CreateAstronautDutyDto { Name = "John", DutyDescription = "Repair" };

            _mockService.Setup(s => s.AddAstronautDutyAsync(request.Name, request.DutyDescription))
                        .ReturnsAsync((AstronautDuty?)null);

            var result = await _controller.CreateAstronautDuty(request);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to add duty.", badRequest.Value);
        }

        [Fact]
        public async Task CreateAstronautDuty_ReturnsOk_WhenServiceSucceeds()
        {
            var request = new CreateAstronautDutyDto { Name = "John", DutyDescription = "Repair" };
            var duty = new AstronautDuty
            {
                Id = 1,
                PersonId = 42,
                Rank = "Lieutenant",
                DutyTitle = request.DutyDescription,
                DutyStartDate = DateTime.UtcNow
            };
            _mockService.Setup(s => s.AddAstronautDutyAsync(request.Name, request.DutyDescription))
                        .ReturnsAsync(duty);

            var result = await _controller.CreateAstronautDuty(request);

            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async Task CreateAstronautDuty_LogsInfo_OnSuccess()
        {
            var dto = new CreateAstronautDutyDto { Name = "Sam", DutyDescription = "Pilot" };
            var duty = new AstronautDuty
            {
                Id = 1,
                PersonId = 42,
                Rank = "Lieutenant",
                DutyTitle = dto.DutyDescription,
                DutyStartDate = DateTime.UtcNow
            };
            _mockService.Setup(s => s.AddAstronautDutyAsync(dto.Name, dto.DutyDescription)).ReturnsAsync(duty);

            var result = await _controller.CreateAstronautDuty(dto);

            Assert.IsType<OkObjectResult>(result);
            _mockLogRepo.Verify(r => r.AddLogAsync(It.Is<ProcessLog>(
                log => log.Level == "INFO" && log.Message.Contains("Created Sam successfully")
            )), Times.Once);
        }

        [Fact]
        public async Task CreateAstronautDuty_LogsError_OnFailure()
        {
            var dto = new CreateAstronautDutyDto { Name = "Sam", DutyDescription = "Pilot" };

            _mockService.Setup(s => s.AddAstronautDutyAsync(dto.Name, dto.DutyDescription)).ReturnsAsync((AstronautDuty?)null);

            var result = await _controller.CreateAstronautDuty(dto);

            Assert.IsType<BadRequestObjectResult>(result);
            _mockLogRepo.Verify(r => r.AddLogAsync(It.Is<ProcessLog>(
                log => log.Level == "ERROR" && log.Message.Contains("Failed to create Sam")
            )), Times.Once);
        }
    }
}
