using Xunit;
using Moq;
using StargateAPI.Services.Impl;
using StargateApi.Repositories;   // repository interface
using System.Threading.Tasks;
namespace UnitTests.Services
{
    public class AstronautDetailServiceTests
    {
        private readonly Mock<IAstronautDetailRepository> _mockRepository;
        private readonly AstronautDetailService _service;

        public AstronautDetailServiceTests()
        {
            _mockRepository = new Mock<IAstronautDetailRepository>();
            _service = new AstronautDetailService(_mockRepository.Object);
        }

        [Fact]
        public async Task CreateAstronautDetail_ReturnsTrue_WhenRepositorySucceeds()
        {
            // Arrange
            _mockRepository.Setup(r => r.CreateAstronautDetail("Jack", "Colonel", "Leader",It.IsAny<DateTime>()))
                           .ReturnsAsync(true);

            // Act
            var result = await _service.CreateAstronautDetail("Jack", "Colonel", "Leader",DateTime.UtcNow);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(r => r.CreateAstronautDetail("Jack", "Colonel", "Leader",It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public async Task CreateAstronautDetail_ReturnsFalse_WhenRepositoryFails()
        {
            // Arrange
            _mockRepository.Setup(r => r.CreateAstronautDetail("Sam", "Major", "Scientist",DateTime.UtcNow))
                           .ReturnsAsync(false);

            // Act
            var result = await _service.CreateAstronautDetail("Sam", "Major", "Scientist",DateTime.UtcNow);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateAstronautDetailAsync_ReturnsTrue_WhenRepositorySucceeds()
        {
            // Arrange
            var dto = new AstronautDetailRequestDto { Name = "Teal'c", CurrentRank = "Warrior", CurrentDutyTitle = "Defender" };
            _mockRepository.Setup(r => r.UpdateAstronautDetailAsync(dto))
                           .ReturnsAsync(true);

            // Act
            var result = await _service.UpdateAstronautDetailAsync(dto);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(r => r.UpdateAstronautDetailAsync(dto), Times.Once);
        }

        [Fact]
        public async Task UpdateAstronautDetailAsync_ReturnsFalse_WhenRepositoryFails()
        {
            // Arrange
            var dto = new AstronautDetailRequestDto { Name = "Daniel", CurrentRank = "Doctor", CurrentDutyTitle = "Scholar" };
            _mockRepository.Setup(r => r.UpdateAstronautDetailAsync(dto))
                           .ReturnsAsync(false);

            // Act
            var result = await _service.UpdateAstronautDetailAsync(dto);

            // Assert
            Assert.False(result);
        }
    }
}
