using Xunit;
using Moq;
using Stargate.Services.Impl;
using Stargate.Repositories;
using StargateAPI.Business.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests.Services
{
    public class AstronautDutyServiceTests
    {
        private readonly Mock<IAstronautDutyRepository> _mockDutyRepo;
        private readonly Mock<IPersonRepository> _mockPersonRepo;
        private readonly AstronautDutyService _service;

        public AstronautDutyServiceTests()
        {
            _mockDutyRepo = new Mock<IAstronautDutyRepository>();
            _mockPersonRepo = new Mock<IPersonRepository>();
            _service = new AstronautDutyService(_mockDutyRepo.Object, _mockPersonRepo.Object);
        }

        [Fact]
        public async Task GetDutiesByAstronautNameAsync_ReturnsDuties()
        {
            // Arrange
            var name = "Jack";
            var duties = new List<AstronautDuty>
            {
                new AstronautDuty { Id = 1, PersonId = 1, DutyTitle = "Mission Leader" },
                new AstronautDuty { Id = 2, PersonId = 1, DutyTitle = "Trainer" }
            };
            _mockDutyRepo.Setup(r => r.GetDutiesByAstronautNameAsync(name))
                         .ReturnsAsync(duties);

            // Act
            var result = await _service.GetDutiesByAstronautNameAsync(name);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, d => d.DutyTitle == "Mission Leader");
            Assert.Contains(result, d => d.DutyTitle == "Trainer");
        }

        [Fact]
        public async Task AddAstronautDutyAsync_ReturnsNull_WhenPersonNotFound()
        {
            // Arrange
            _mockPersonRepo.Setup(r => r.GetPersonByNameAsync("Daniel"))
                           .ReturnsAsync((Person?)null);

            // Act
            var result = await _service.AddAstronautDutyAsync("Daniel", "Test Duty");

            // Assert
            Assert.Null(result);
            _mockDutyRepo.Verify(r => r.AddAstronautDutyAsync(It.IsAny<AstronautDuty>()), Times.Never);
        }

        [Fact]
        public async Task AddAstronautDutyAsync_ReturnsFalse_WhenAstronautDetailIsNull()
        {
            // Arrange
            var person = new Person { Id = 1, Name = "Jack", AstronautDetail = null };
            _mockPersonRepo.Setup(r => r.GetPersonByNameAsync("Jack"))
                           .ReturnsAsync(person);

            // Act
            var result = await _service.AddAstronautDutyAsync("Jack", "Test Duty");

            // Assert
            Assert.Null(result);
            _mockDutyRepo.Verify(r => r.AddAstronautDutyAsync(It.IsAny<AstronautDuty>()), Times.Never);
        }

        [Fact]
        public async Task AddAstronautDutyAsync_CreatesDuty_AndReturnsTrue()
        {
            // Arrange
            var person = new Person
            {
                Id = 42,
                Name = "Teal'c",
                AstronautDetail = new AstronautDetail { CurrentRank = "Jaffa Warrior" }
            };

            _mockPersonRepo.Setup(r => r.GetPersonByNameAsync("Teal'c"))
                           .ReturnsAsync(person);

            
            var duty = new AstronautDuty
            {
                Id = 1,
                PersonId = 42,
                Rank = "Lieutenant",
                DutyTitle = "Title",
                DutyStartDate = DateTime.UtcNow
            };

            _mockDutyRepo.Setup(r => r.AddAstronautDutyAsync(It.IsAny<AstronautDuty>()))
                         .ReturnsAsync(duty);

            // Act
            var result = await _service.AddAstronautDutyAsync("Teal'c", "Defend Earth");

            // Assert
            Assert.NotNull(result);
            _mockDutyRepo.Verify(r => r.AddAstronautDutyAsync(It.Is<AstronautDuty>(
                d => d.PersonId == 42 &&
                     d.Rank == "Jaffa Warrior" &&
                     d.DutyTitle == "Defend Earth"
            )), Times.Once);
        }
    }
}
