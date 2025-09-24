using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
namespace Stargate.Repositories.Impl
{
    public class AstronautDutyRepository : IAstronautDutyRepository
    {
        private readonly StargateContext _context;

        public AstronautDutyRepository(StargateContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AstronautDuty>> GetDutiesByAstronautNameAsync(string name)
        {
            Person? person = await _context.People.FirstOrDefaultAsync(p => p.Name == name);
            if (person == null)
            {
                return Enumerable.Empty<AstronautDuty>();
            }
            return await _context.AstronautDuties
                .Where(d => d.PersonId == person.Id)
                .OrderByDescending(d => d.DutyStartDate)
                .ToListAsync();
        }

        public async Task AddAstronautDutyAsync(AstronautDuty duty)
        {
            await _context.AstronautDuties.AddAsync(duty);
            await _context.SaveChangesAsync();
        }
    }
}