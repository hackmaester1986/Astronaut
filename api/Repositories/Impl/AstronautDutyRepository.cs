using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
namespace Stargate.Repositories.Impl
{
    public class AstronautDutyRepository : IAstronautDutyRepository
    {
        private readonly StargateContext _context;
        private readonly ILogger<AstronautDutyRepository> _logger;
        public AstronautDutyRepository(StargateContext context, ILogger<AstronautDutyRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<AstronautDuty>> GetDutiesByAstronautNameAsync(string name)
        {
            Person? person = await _context.People.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());
            if (person == null)
            {
                _logger.LogError("A person with name {Name} doesn't exist", name);
                return Enumerable.Empty<AstronautDuty>();
            }
            return await _context.AstronautDuties
                .Where(d => d.PersonId == person.Id)
                .OrderBy(d => d.DutyStartDate)
                .ToListAsync();
        }

        public async Task<AstronautDuty> AddAstronautDutyAsync(AstronautDuty duty)
        {
            bool hasDuties = await _context.AstronautDuties
            .Where(d => d.PersonId == duty.PersonId)
            .AnyAsync();

            if (hasDuties)
            {
                AstronautDuty? firstDuty = await _context.AstronautDuties
                 .Where(d => d.PersonId == duty.PersonId)
                 .OrderByDescending(d => d.DutyStartDate)
                 .FirstOrDefaultAsync();
                if(firstDuty is not null)
                    firstDuty.DutyEndDate = DateTime.UtcNow.AddDays(-1);
            }
            AstronautDetail? detail = await _context.AstronautDetails.FirstOrDefaultAsync(d => d.PersonId == duty.PersonId);
            if (detail is null)
            {
                _logger.LogError("Cannot add a new Astronaut Duty to id: {Id} because they don't have an Astronaut detail",duty.PersonId);
                return null;
            }

            if (duty.DutyTitle.ToLower() == "retired")
                {
                    detail.CareerEndDate = DateTime.UtcNow.AddDays(-1);
                }
            detail.CurrentDutyTitle = duty.DutyTitle;
            await _context.AstronautDuties.AddAsync(duty);
            await _context.SaveChangesAsync();
            return duty;
        }
    }
}