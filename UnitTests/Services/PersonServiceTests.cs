using Xunit;
using Moq;
using Stargate.Services.Impl;
using Stargate.Repositories;
using StargateAPI.Business.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests.Services
{
    public class PersonServiceTests
    {
        private readonly Mock<IPersonRepository> _mockRepository;
        private readonly PersonService _service;

        public PersonServiceTests()
        {
            _mockRepository = new Mock<IPersonRepository>();
            _service = new PersonService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllPeopleAsync_ReturnsPeopleList()
        {
            // Arrange
            var people = new List<Person>
            {
                new Person { Id = 1, Name = "Jack" },
                new Person { Id = 2, Name = "Sam" }
            };

            _mockRepository.Setup(r => r.GetAllPeopleAsync())
                           .ReturnsAsync(people);

            // Act
            var result = await _service.GetAllPeopleAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, p => p.Name == "Jack");
            Assert.Contains(result, p => p.Name == "Sam");
        }

        [Fact]
        public async Task GetPersonByNameAsync_ReturnsPerson_WhenFound()
        {
            // Arrange
            var person = new Person { Id = 1, Name = "Teal'c" };
            _mockRepository.Setup(r => r.GetPersonByNameAsync("Teal'c"))
                           .ReturnsAsync(person);

            // Act
            var result = await _service.GetPersonByNameAsync("Teal'c");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Teal'c", result.Name);
        }

        [Fact]
        public async Task GetPersonByNameAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetPersonByNameAsync("Daniel"))
                           .ReturnsAsync((Person?)null);

            // Act
            var result = await _service.GetPersonByNameAsync("Daniel");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddPersonAsync_CreatesPerson_AndReturnsTrue_WhenRepositorySucceeds()
        {
            // Arrange
            var name = "Hammond";
            _mockRepository.Setup(r => r.AddPersonAsync(It.Is<Person>(p => p.Name == name)))
                           .ReturnsAsync(true);

            // Act
            var result = await _service.AddPersonAsync(name);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(r => r.AddPersonAsync(It.Is<Person>(p => p.Name == name)), Times.Once);
        }

        [Fact]
        public async Task AddPersonAsync_ReturnsFalse_WhenRepositoryFails()
        {
            // Arrange
            var name = "Hammond";
            _mockRepository.Setup(r => r.AddPersonAsync(It.IsAny<Person>()))
                           .ReturnsAsync(false);

            // Act
            var result = await _service.AddPersonAsync(name);

            // Assert
            Assert.False(result);
        }
    }
}
