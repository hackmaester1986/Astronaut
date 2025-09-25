using StargateAPI.Business.Data;

namespace Stargate.Services
{
    public interface IAstronautDutyService
    {
        Task<IEnumerable<AstronautDuty>> GetDutiesByAstronautNameAsync(string name);
        Task<AstronautDuty> AddAstronautDutyAsync(string name, string dutyDescription);
    }
}