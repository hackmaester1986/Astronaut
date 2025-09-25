using Stargate.Repositories;
using StargateAPI.Business.Data;

namespace Stargate.Services.Impl
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<IEnumerable<Person>> GetAllPeopleAsync()
        {
            return await _personRepository.GetAllPeopleAsync();
        }

        public async Task<Person?> GetPersonByNameAsync(string name)
        {
            return await _personRepository.GetPersonByNameAsync(name);
        }

        public async Task<bool> AddPersonAsync(string name)
        {
            var person = new Person { Name = name };
            return await _personRepository.AddPersonAsync(person);
        }
    }
}