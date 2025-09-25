using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
namespace Stargate.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly StargateContext _context;
        private readonly ILogger<PersonRepository> _logger;
        public PersonRepository(StargateContext context, ILogger<PersonRepository> logger)
        {
            _context = context;
            _logger = logger;
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
                .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> AddPersonAsync(Person person)
        {
            bool any = _context.People
                .Where(p => p.Name.ToLower() == person.Name.ToLower())
                .Any();
            if (any)
            {
                _logger.LogInformation("A person with the name {Name} already exists",person.Name);
                return false;
            }
            await _context.People.AddAsync(person);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}