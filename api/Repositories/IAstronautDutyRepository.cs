using StargateAPI.Business.Data;

namespace Stargate.Repositories
{
    public interface IAstronautDutyRepository
    {
        Task<IEnumerable<AstronautDuty>> GetDutiesByAstronautNameAsync(string name);
        Task AddAstronautDutyAsync(AstronautDuty duty);
    }
}