using Stargate.Repositories;
using StargateAPI.Business.Data;
namespace Stargate.Services.Impl
{
    public class AstronautDutyService : IAstronautDutyService
    {
        private readonly IAstronautDutyRepository _astronautDutyRepository;
        private readonly IPersonRepository _personRepository;
        public AstronautDutyService(
            IAstronautDutyRepository astronautDutyRepository,
            IPersonRepository personRepository
        )
        {
            _personRepository = personRepository;
            _astronautDutyRepository = astronautDutyRepository;
        }

        public async Task<IEnumerable<AstronautDuty>> GetDutiesByAstronautNameAsync(string name)
        {
            return await _astronautDutyRepository.GetDutiesByAstronautNameAsync(name);
        }

        public async Task<AstronautDuty> AddAstronautDutyAsync(string name, string dutyDescription)
        {
            Person? person = await _personRepository.GetPersonByNameAsync(name);
            if (person == null || person.AstronautDetail == null) return null;
            var duty = new AstronautDuty
            {
                PersonId = person.Id,
                Rank = person.AstronautDetail!.CurrentRank,
                DutyTitle = dutyDescription,
                DutyStartDate = DateTime.UtcNow,
            };
            var addedDuty = await _astronautDutyRepository.AddAstronautDutyAsync(duty);
            return addedDuty;
        }
    }
}