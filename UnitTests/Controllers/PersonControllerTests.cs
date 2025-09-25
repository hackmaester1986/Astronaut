using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Controllers;
using Stargate.Services;
using StargateAPI.Business.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stargate.Repositories;

namespace StargateAPI.Tests.Controllers
{
    public class PersonControllerTests
    {
        private readonly Mock<IPersonService> _mockService;
        private readonly PersonController _controller;
        private readonly Mock<IProcessLogRepository> _mockLogRepo;

        public PersonControllerTests()
        {
            _mockService = new Mock<IPersonService>();
            _mockLogRepo = new Mock<IProcessLogRepository>();
            _controller = new PersonController(_mockService.Object, _mockLogRepo.Object);
        }

        [Fact]
        public async Task GetPeople_ReturnsOk_WithListOfPeople()
        {
            // Arrange
            var people = new List<Person>
            {
                new Person { Id = 1, Name = "Jack" },
                new Person { Id = 2, Name = "Sam" }
            };
            _mockService.Setup(s => s.GetAllPeopleAsync())
                        .ReturnsAsync(people);

            // Act
            var result = await _controller.GetPeople();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Person>>(okResult.Value);
            Assert.Collection(returnValue,
                p => Assert.Equal("Jack", p.Name),
                p => Assert.Equal("Sam", p.Name));
        }

        [Fact]
        public async Task GetPersonByName_WhenPersonExists_ReturnsOk()
        {
            // Arrange
            var person = new Person { Id = 1, Name = "Teal'c" };
            _mockService.Setup(s => s.GetPersonByNameAsync("Teal'c"))
                        .ReturnsAsync(person);

            // Act
            var result = await _controller.GetPersonByName("Teal'c");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Person>(okResult.Value);
            Assert.Equal("Teal'c", returnValue.Name);
        }

        [Fact]
        public async Task GetPersonByName_WhenPersonDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetPersonByNameAsync("Daniel"))
                        .ReturnsAsync((Person?)null);

            // Act
            var result = await _controller.GetPersonByName("Daniel");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreatePerson_ReturnsCreatedAtAction()
        {
            // Arrange
            var name = "Hammond";
            _mockService.Setup(s => s.AddPersonAsync(name))
                        .ReturnsAsync(true);

            // Act
            var result = await _controller.CreatePerson(name);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(PersonController.GetPersonByName), createdResult.ActionName);
            Assert.Equal(name, createdResult.RouteValues["name"]);
        }
        
        [Fact]
        public async Task CreatePerson_LogsInfo_OnSuccess()
        {
            // Arrange
            var name = "Jack";
            _mockService.Setup(s => s.AddPersonAsync(name)).ReturnsAsync(true);

            // Act
            var result = await _controller.CreatePerson(name);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
            _mockLogRepo.Verify(r => r.AddLogAsync(It.Is<ProcessLog>(
                log => log.Level == "INFO" && log.Message.Contains("added successfully")
            )), Times.Once);
        }

        [Fact]
        public async Task CreatePerson_LogsError_OnFailure()
        {
            var name = "Jack";
            _mockService.Setup(s => s.AddPersonAsync(name)).ReturnsAsync(false);

            var result = await _controller.CreatePerson(name);

            Assert.IsType<BadRequestObjectResult>(result);
            _mockLogRepo.Verify(r => r.AddLogAsync(It.Is<ProcessLog>(
                log => log.Level == "ERROR" && log.Message.Contains("Failed to add person")
            )), Times.Once);
        }
    }
}