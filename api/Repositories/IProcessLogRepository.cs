using StargateAPI.Business.Data;

namespace Stargate.Repositories
{
    public interface IProcessLogRepository
    {
        Task AddLogAsync(ProcessLog log);
    }
}
