using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stargate.Repositories;
using Stargate.Services;
using StargateAPI.Business.Data;
using System.Net;

namespace StargateAPI.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly IProcessLogRepository _logRepository;
        public PersonController(IPersonService personService, IProcessLogRepository processLogRepository)
        {
            _personService = personService;
            _logRepository = processLogRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetPeople()
        {
            List<Person> people = (await _personService.GetAllPeopleAsync()).ToList();
            return Ok(people);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetPersonByName(string name)
        {
            Person? person = await _personService.GetPersonByNameAsync(name);
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }

        [HttpPost("")]
        public async Task<IActionResult> CreatePerson([FromBody] string name)
        {
            bool success = await _personService.AddPersonAsync(name);
            await _logRepository.AddLogAsync(new ProcessLog
            {
                Level = success?"INFO":"ERROR",
                Message = success ? $"Person {name} added successfully." : $"Failed to add person {name}.",
                Context = "PersonController.CreatePerson"
            });
            if (!success) return BadRequest("Failed to add person");
            
            return CreatedAtAction(nameof(GetPersonByName), new { name = name }, null);

        }
    }
}