using Xunit;
using Microsoft.EntityFrameworkCore;
using Stargate.Repositories;
using StargateAPI.Business.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Repositories
{
    public class PersonRepositoryTests
    {
        private readonly Mock<ILogger<PersonRepository>> _mockLogger;

        public PersonRepositoryTests()
        {
            _mockLogger = new Mock<ILogger<PersonRepository>>();
        }
        private DbContextOptions<StargateContext> GetInMemoryOptions()
        {
            return new DbContextOptionsBuilder<StargateContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // unique DB per test
                .Options;
        }

        [Fact]
        public async Task GetAllPeopleAsync_ReturnsAllPeople()
        {
            using var context = new StargateContext(GetInMemoryOptions());
            context.People.Add(new Person
            {
                Name = "Jack",
                AstronautDetail = new AstronautDetail { CurrentRank = "Colonel", CurrentDutyTitle = "Leader" },
                AstronautDuties = new List<AstronautDuty>
                {
                    new AstronautDuty { DutyTitle = "Mission Leader", DutyStartDate = DateTime.UtcNow }
                }
            });
            context.People.Add(new Person { Name = "Sam" });
            await context.SaveChangesAsync();

            var repo = new PersonRepository(context,_mockLogger.Object);

            var result = await repo.GetAllPeopleAsync();

            Assert.Equal(2, result.Count());
            Assert.Contains(result, p => p.Name == "Jack" && p.AstronautDetail != null);
            Assert.Contains(result, p => p.Name == "Sam");
        }

        [Fact]
        public async Task GetPersonByNameAsync_ReturnsPerson_WhenFound()
        {
            using var context = new StargateContext(GetInMemoryOptions());
            var person = new Person { Name = "Daniel" };
            context.People.Add(person);
            await context.SaveChangesAsync();

            var repo = new PersonRepository(context,_mockLogger.Object);

            var result = await repo.GetPersonByNameAsync("Daniel");

            Assert.NotNull(result);
            Assert.Equal("Daniel", result.Name);
        }

        [Fact]
        public async Task GetPersonByNameAsync_ReturnsNull_WhenNotFound()
        {
            using var context = new StargateContext(GetInMemoryOptions());
            var repo = new PersonRepository(context,_mockLogger.Object);

            var result = await repo.GetPersonByNameAsync("Nonexistent");

            Assert.Null(result);
        }

        [Fact]
        public async Task AddPersonAsync_ReturnsTrue_WhenAdded()
        {
            using var context = new StargateContext(GetInMemoryOptions());
            var repo = new PersonRepository(context,_mockLogger.Object);

            var result = await repo.AddPersonAsync(new Person { Name = "Hammond" });

            Assert.True(result);
            Assert.Single(context.People);
        }

        [Fact]
        public async Task AddPersonAsync_ReturnsFalse_WhenDuplicate()
        {
            using var context = new StargateContext(GetInMemoryOptions());
            context.People.Add(new Person { Name = "Teal'c" });
            await context.SaveChangesAsync();

            var repo = new PersonRepository(context,_mockLogger.Object);

            var result = await repo.AddPersonAsync(new Person { Name = "Teal'c" });

            Assert.False(result);
            Assert.Single(context.People); // still only one
        }
    }
}
