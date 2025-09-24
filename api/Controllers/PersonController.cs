using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stargate.Services;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;
using System.Net;

namespace StargateAPI.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;
        public PersonController(IPersonService personService)
        {   
            _personService = personService;
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
            await _personService.AddPersonAsync(name);
            return CreatedAtAction(nameof(GetPersonByName), new { name = name }, null);

        }
    }
}