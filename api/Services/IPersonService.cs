using StargateAPI.Business.Data;

namespace Stargate.Services
{
    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetAllPeopleAsync();
        Task<Person?> GetPersonByNameAsync(string name);
        Task AddPersonAsync(string name);
    }
}