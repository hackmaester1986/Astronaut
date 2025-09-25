using Microsoft.EntityFrameworkCore;
using StargateApi.Repositories;
using StargateAPI.Business.Data;

namespace StargateAPI.Repositories.Impl

{
    public class AstronautDetailRepository : IAstronautDetailRepository
    {
        private readonly StargateContext _context;
        private readonly ILogger<AstronautDetailRepository> _logger;
        public AstronautDetailRepository(StargateContext context, ILogger<AstronautDetailRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> CreateAstronautDetail(string name, string rank, string title, DateTime? careerStartDate)
        {
            var person = await _context.People
                .Include(p => p.AstronautDetail)
                .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());
            if (person == null || person.AstronautDetail is not null)
            {
                _logger.LogInformation("A person with the name {Name} already has an astronaut detail", name);
                return false;
            }

            var astronautDetail = new AstronautDetail
            {
                PersonId = person.Id,
                CurrentRank = rank,
                CurrentDutyTitle = title,
                CareerStartDate = careerStartDate ?? DateTime.UtcNow
            };

            _context.AstronautDetails.Add(astronautDetail);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Astronaut detail for {Name} created successfully", name);
            return true;
        }

        public async Task<bool> UpdateAstronautDetailAsync(AstronautDetailRequestDto updatedDetail)
        {
            var person = await _context.People.FirstOrDefaultAsync(p => p.Name.ToLower() == updatedDetail.Name.ToLower());
            if (person == null)
            {
                _logger.LogInformation("A person with the name {Name} doesn't exist", updatedDetail.Name);
                return false;
            }
            var existing = await _context.AstronautDetails
                .FirstOrDefaultAsync(a => a.Id == person.Id);

            if (existing == null)
            {
                _logger.LogInformation("{Name} already has Astronaut details", updatedDetail.Name);
                return false;
            }
            bool any = false;
            if (!string.IsNullOrEmpty(updatedDetail.CurrentRank))
            {
                existing.CurrentRank = updatedDetail.CurrentRank;
                any = true;
            }
            if (!string.IsNullOrEmpty(updatedDetail.CurrentDutyTitle))
            {
                existing.CurrentDutyTitle = updatedDetail.CurrentDutyTitle;
                any = true;
            }
            if (!any) return false;
            _logger.LogInformation("Successfully updated the Astronaut detail of {Name}", updatedDetail.Name);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<AstronautDetailDto> GetDetailByName(string name)
        {
            var person = await _context.People.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());
            if (person is null) return null;

            var detail = await _context.AstronautDetails.FirstOrDefaultAsync(d => d.PersonId == person.Id);
            if (detail is null) return null;

            AstronautDetailDto dto = new AstronautDetailDto
            {
                Name = name,
                CurrentRank = detail.CurrentRank,
                CurrentDutyTitle = detail.CurrentDutyTitle,
                CareerStartDate = detail.CareerStartDate,
                CareerEndDate = detail.CareerEndDate ?? null
            };
            return dto;
        }
    }
}