using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
namespace Stargate.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly StargateContext _context;

        public PersonRepository(StargateContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Person>> GetAllPeopleAsync()
        {
            return await _context.People
                .Include(p => p.AstronautDetail)
                .Include(p => p.AstronautDuties)
                .ToListAsync();
        }

        public async Task<Person?> GetPersonByNameAsync(string name)
        {
            return await _context.People
                .Include(p => p.AstronautDetail)
                .Include(p => p.AstronautDuties)
                .FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task AddPersonAsync(Person person)
        {
            await _context.People.AddAsync(person);
            await _context.SaveChangesAsync();
        }
    }

}