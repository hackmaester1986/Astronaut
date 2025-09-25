using Xunit;
using Microsoft.EntityFrameworkCore;
using Stargate.Repositories.Impl; // adjust namespace if needed
using StargateAPI.Business.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests.Repositories
{
    public class ProcessLogRepositoryTests
    {
        private DbContextOptions<StargateContext> GetInMemoryOptions()
        {
            return new DbContextOptionsBuilder<StargateContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // fresh DB per test
                .Options;
        }

        [Fact]
        public async Task AddLogAsync_WritesLogToDatabase()
        {
            // Arrange
            var options = GetInMemoryOptions();
            using var context = new StargateContext(options);
            var repo = new ProcessLogRepository(context);

            var log = new ProcessLog
            {
                Level = "INFO",
                Message = "Test log entry",
                Context = "UnitTest",
                Timestamp = DateTime.UtcNow
            };

            // Act
            await repo.AddLogAsync(log);

            // Assert
            var logs = await context.ProcessLogs.ToListAsync();
            Assert.Single(logs);

            var savedLog = logs.First();
            Assert.Equal("INFO", savedLog.Level);
            Assert.Equal("Test log entry", savedLog.Message);
            Assert.Equal("UnitTest", savedLog.Context);
        }
    }
}
