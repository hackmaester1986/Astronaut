using Xunit;
using Microsoft.EntityFrameworkCore;
using Stargate.Repositories.Impl;
using StargateAPI.Business.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace UnitTests.Repositories
{
    public class AstronautDutyRepositoryTests
    {
        private readonly Mock<ILogger<AstronautDutyRepository>> _mockLogger;

        public AstronautDutyRepositoryTests()
        {
            _mockLogger = new Mock<ILogger<AstronautDutyRepository>>();
        }
        private DbContextOptions<StargateContext> GetInMemoryOptions()
        {
            return new DbContextOptionsBuilder<StargateContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // unique DB per test
                .Options;
        }

        [Fact]
        public async Task GetDutiesByAstronautNameAsync_ReturnsEmpty_WhenPersonNotFound()
        {
            using var context = new StargateContext(GetInMemoryOptions());
            var repo = new AstronautDutyRepository(context,_mockLogger.Object);

            var result = await repo.GetDutiesByAstronautNameAsync("Jack");

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetDutiesByAstronautNameAsync_ReturnsOrderedDuties_WhenPersonExists()
        {
            using var context = new StargateContext(GetInMemoryOptions());
            var person = new Person { Name = "Jack" };
            context.People.Add(person);
            await context.SaveChangesAsync();

            var olderDuty = new AstronautDuty { PersonId = person.Id, DutyTitle = "Trainer", DutyStartDate = DateTime.UtcNow.AddDays(-10) };
            var newerDuty = new AstronautDuty { PersonId = person.Id, DutyTitle = "Leader", DutyStartDate = DateTime.UtcNow };

            context.AstronautDuties.AddRange(olderDuty, newerDuty);
            await context.SaveChangesAsync();

            var repo = new AstronautDutyRepository(context,_mockLogger.Object);

            var result = await repo.GetDutiesByAstronautNameAsync("Jack");

            var duties = result.ToList();
            Assert.Equal(2, duties.Count);
            Assert.Equal("Leader", duties[1].DutyTitle); // older first
            Assert.Equal("Trainer", duties[0].DutyTitle);
        }

        [Fact]
        public async Task AddAstronautDutyAsync_ReturnsNull_WhenAstronautDetailMissing()
        {
            using var context = new StargateContext(GetInMemoryOptions());
            var person = new Person { Name = "Sam" };
            context.People.Add(person);
            await context.SaveChangesAsync();

            var duty = new AstronautDuty { PersonId = person.Id, DutyTitle = "Pilot", DutyStartDate = DateTime.UtcNow };

            var repo = new AstronautDutyRepository(context,_mockLogger.Object);

            var result = await repo.AddAstronautDutyAsync(duty);

            Assert.Null(result);
        }

        [Fact]
        public async Task AddAstronautDutyAsync_EndsPreviousDuty_WhenExists()
        {
            using var context = new StargateContext(GetInMemoryOptions());
            var person = new Person
            {
                Name = "Teal'c",
                AstronautDetail = new AstronautDetail { CurrentRank = "Warrior", CurrentDutyTitle = "Guard" }
            };
            context.People.Add(person);
            await context.SaveChangesAsync();

            var oldDuty = new AstronautDuty { PersonId = person.Id, DutyTitle = "Trainer", DutyStartDate = DateTime.UtcNow.AddDays(-5) };
            context.AstronautDuties.Add(oldDuty);
            await context.SaveChangesAsync();

            var newDuty = new AstronautDuty { PersonId = person.Id, DutyTitle = "Leader", DutyStartDate = DateTime.UtcNow };
            var repo = new AstronautDutyRepository(context,_mockLogger.Object);

            var result = await repo.AddAstronautDutyAsync(newDuty);

            Assert.NotNull(result);
            Assert.Equal(newDuty.DutyTitle, result.DutyTitle);
            Assert.Equal(newDuty.PersonId, result.PersonId);

            var updatedOld = await context.AstronautDuties.FindAsync(oldDuty.Id);
            Assert.NotNull(updatedOld.DutyEndDate); // old duty ended
        }

        [Fact]
        public async Task AddAstronautDutyAsync_SetsCareerEndDate_WhenRetired()
        {
            using var context = new StargateContext(GetInMemoryOptions());
            var person = new Person
            {
                Name = "Hammond",
                AstronautDetail = new AstronautDetail { CurrentRank = "General", CurrentDutyTitle = "Commander" }
            };
            context.People.Add(person);
            await context.SaveChangesAsync();

            var duty = new AstronautDuty { PersonId = person.Id, DutyTitle = "Retired", DutyStartDate = DateTime.UtcNow };
            var repo = new AstronautDutyRepository(context,_mockLogger.Object);

            var result = await repo.AddAstronautDutyAsync(duty);

            Assert.NotNull(result);
            var detail = await context.AstronautDetails.FirstAsync(d => d.PersonId == person.Id);
            Assert.NotNull(detail.CareerEndDate); // CareerEndDate set
            Assert.Equal("Retired", detail.CurrentDutyTitle);
        }
    }
}
