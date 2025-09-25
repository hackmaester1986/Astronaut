using StargateAPI.Business.Data;

namespace Stargate.Repositories.Impl
{
    public class ProcessLogRepository : IProcessLogRepository
    {
        private readonly StargateContext _context;
        public ProcessLogRepository(StargateContext context) => _context = context;

        public async Task AddLogAsync(ProcessLog log)
        {
            await _context.ProcessLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}