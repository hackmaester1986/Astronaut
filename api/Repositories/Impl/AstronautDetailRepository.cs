using Microsoft.EntityFrameworkCore;
using StargateApi.Repositories;
using StargateAPI.Business.Data;

namespace StargateAPI.Repositories.Impl

{
    public class AstronautDetailRepository : IAstronautDetailRepository
    {
        private readonly StargateContext _context;

        public AstronautDetailRepository(StargateContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAstronautDetail(string name, string rank, string title)
        {
            var person = await _context.People.FirstOrDefaultAsync(p => p.Name == name);
            if (person == null)
            {
                return false;
            }

            var astronautDetail = new AstronautDetail
            {
                PersonId = person.Id,
                CurrentRank = rank,
                CurrentDutyTitle = title,
                CareerStartDate = DateTime.UtcNow
            };

            _context.AstronautDetails.Add(astronautDetail);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}