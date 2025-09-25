using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stargate.Repositories;
using Stargate.Services;
using StargateAPI.Business.Data;
using System.Diagnostics;
using System.Net;

namespace StargateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AstronautDutyController : ControllerBase
    {
        private readonly IAstronautDutyService _astronautDutyService;
        private readonly IProcessLogRepository _logRepository;
        public AstronautDutyController(IAstronautDutyService astronautDutyService, IProcessLogRepository processLogRepository)
        {
            _astronautDutyService = astronautDutyService;
            _logRepository = processLogRepository;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetAstronautDutiesByName(string name)
        {
            var duties = await _astronautDutyService.GetDutiesByAstronautNameAsync(name);
            if (duties.Count() == 0)
            {
                await _logRepository.AddLogAsync(new ProcessLog
                {
                    Level = "WARNING",
                    Message = $"There are no duties for {name}.",
                    Context = "AstronautDutyController.GetDutiesByAstronautNameAsync"
                });
                return NotFound();
            }
            return Ok(duties);        
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAstronautDuty([FromBody] CreateAstronautDutyDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.DutyDescription))
            {
                return BadRequest("Invalid request data.");
            }

            var addedDuty = await _astronautDutyService.AddAstronautDutyAsync(request.Name, request.DutyDescription);
            await _logRepository.AddLogAsync(new ProcessLog
            {
                Level = (addedDuty is not null)?"INFO":"ERROR",
                Message = (addedDuty is not null)?$"Created {request.Name} successfully":$"Failed to create {request.Name}.",
                Context = "AstronautDutyController.AddAstronautDutyAsync"
            });

            if (addedDuty is null)
            {
                return BadRequest("Failed to add duty.");
            }
            return Ok(addedDuty);
        }
    }
}