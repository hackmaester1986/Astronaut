using StargateAPI.Business.Data;
namespace Stargate.Repositories
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetAllPeopleAsync();
        Task<Person?> GetPersonByNameAsync(string name);
        Task AddPersonAsync(Person person);
    }
}