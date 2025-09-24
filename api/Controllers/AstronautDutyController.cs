using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stargate.Services;
using StargateAPI.Business.Commands;
using System.Net;

namespace StargateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AstronautDutyController : ControllerBase
    {
        private readonly IAstronautDutyService _astronautDutyService;
        public AstronautDutyController(IAstronautDutyService astronautDutyService)
        {
            _astronautDutyService = astronautDutyService;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetAstronautDutiesByName(string name)
        {
            var duties = await _astronautDutyService.GetDutiesByAstronautNameAsync(name);
            return Ok();        
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAstronautDuty([FromBody] CreateAstronautDutyDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.DutyDescription))
            {
                return BadRequest("Invalid request data.");
            }
            bool success = await _astronautDutyService.AddAstronautDutyAsync(request.Name, request.DutyDescription);
            if (!success)
            {
                return BadRequest("Failed to add duty.");
            }
            return Ok();
        }
    }
}