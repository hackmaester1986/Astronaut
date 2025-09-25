using Xunit;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Repositories.Impl;
using StargateAPI.Business.Data;
using System.Threading.Tasks;
using System;
using Moq;
using Microsoft.Extensions.Logging;

namespace UnitTests.Repositories
{
    public class AstronautDetailRepositoryTests
    {
        private readonly Mock<ILogger<AstronautDetailRepository>> _mockLogger;

        public AstronautDetailRepositoryTests()
        {
            _mockLogger = new Mock<ILogger<AstronautDetailRepository>>();
        }
        private DbContextOptions<StargateContext> GetInMemoryOptions()
        {
            return new DbContextOptionsBuilder<StargateContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // unique db per test
                .Options;
        }

        [Fact]
        public async Task CreateAstronautDetail_ReturnsFalse_WhenPersonNotFound()
        {
            using var context = new StargateContext(GetInMemoryOptions());
            var repo = new AstronautDetailRepository(context,_mockLogger.Object);

            var result = await repo.CreateAstronautDetail("Jack", "Colonel", "Leader",DateTime.UtcNow);

            Assert.False(result);
        }

        [Fact]
        public async Task CreateAstronautDetail_ReturnsFalse_WhenPersonAlreadyHasDetail()
        {
            using var context = new StargateContext(GetInMemoryOptions());
            var person = new Person
            {
                Name = "Jack",
                AstronautDetail = new AstronautDetail { CurrentRank = "Colonel", CurrentDutyTitle = "Leader" }
            };
            context.People.Add(person);
            await context.SaveChangesAsync();

            var repo = new AstronautDetailRepository(context,_mockLogger.Object);

            var result = await repo.CreateAstronautDetail("Jack", "Colonel", "Leader",DateTime.UtcNow);

            Assert.False(result);
        }

        [Fact]
        public async Task CreateAstronautDetail_ReturnsTrue_AndPersistsDetail()
        {
            using var context = new StargateContext(GetInMemoryOptions());
            var person = new Person { Name = "Jack" };
            context.People.Add(person);
            await context.SaveChangesAsync();

            var repo = new AstronautDetailRepository(context,_mockLogger.Object);

            var result = await repo.CreateAstronautDetail("Jack", "Colonel", "Leader",DateTime.UtcNow);

            Assert.True(result);
            var detail = await context.AstronautDetails.FirstOrDefaultAsync();
            Assert.NotNull(detail);
            Assert.Equal("Colonel", detail.CurrentRank);
            Assert.Equal("Leader", detail.CurrentDutyTitle);
            Assert.Equal(person.Id, detail.PersonId);
        }

        [Fact]
        public async Task UpdateAstronautDetailAsync_ReturnsFalse_WhenPersonNotFound()
        {
            using var context = new StargateContext(GetInMemoryOptions());
            var repo = new AstronautDetailRepository(context,_mockLogger.Object);

            var dto = new AstronautDetailRequestDto { Name = "Nonexistent", CurrentRank = "Captain" };
            var result = await repo.UpdateAstronautDetailAsync(dto);

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateAstronautDetailAsync_ReturnsFalse_WhenDetailNotFound()
        {
            using var context = new StargateContext(GetInMemoryOptions());
            var person = new Person { Name = "Jack" };
            context.People.Add(person);
            await context.SaveChangesAsync();

            var repo = new AstronautDetailRepository(context,_mockLogger.Object);

            var dto = new AstronautDetailRequestDto { Name = "Jack", CurrentRank = "Captain" };
            var result = await repo.UpdateAstronautDetailAsync(dto);

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateAstronautDetailAsync_ReturnsTrue_AndUpdatesDetail()
        {
            using var context = new StargateContext(GetInMemoryOptions());
            var person = new Person { Name = "Jack" };
            var detail = new AstronautDetail { Id = person.Id, PersonId = person.Id, CurrentRank = "Colonel", CurrentDutyTitle = "Leader" };
            person.AstronautDetail = detail;
            context.People.Add(person);
            context.AstronautDetails.Add(detail);
            await context.SaveChangesAsync();

            var repo = new AstronautDetailRepository(context,_mockLogger.Object);

            var dto = new AstronautDetailRequestDto { Name = "Jack", CurrentRank = "General", CurrentDutyTitle = "Commander" };
            var result = await repo.UpdateAstronautDetailAsync(dto);

            Assert.True(result);
            var updated = await context.AstronautDetails.FirstAsync();
            Assert.Equal("General", updated.CurrentRank);
            Assert.Equal("Commander", updated.CurrentDutyTitle);
        }
    }

}
