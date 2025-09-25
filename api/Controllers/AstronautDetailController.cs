using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stargate.Repositories;
using Stargate.Services;
using StargateAPI.Business.Data;
using StargateAPI.Services;
using System.Net;

namespace StargateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AstronautDetailController : ControllerBase
    {
        private readonly IAstronautDetailService _astronautDetailService;
        private readonly IProcessLogRepository _logRepository;
        public AstronautDetailController(IAstronautDetailService astronautDetailService, IProcessLogRepository logRepository)
        {
            _astronautDetailService = astronautDetailService;
            _logRepository = logRepository;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAstronautDetail([FromBody] CreateAstronautDetailDto request)
        {
            if (request == null || request.CareerStartDate is null || string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Rank))
            {
                return BadRequest("Invalid request data.");
            }
            bool success = await _astronautDetailService.CreateAstronautDetail(request.Name, request.Rank, request.Title,request.CareerStartDate);

            await _logRepository.AddLogAsync(new ProcessLog
            {
                Level = success ? "INFO" : "ERROR",
                Message = success ? $"Created {request.Name} astronaut detail" : $"Failed to create {request.Name} astronaut detail.",
                Context = "AstronautDetailController.CreateAstronautDetail"
            });

            if (!success)
            {
                return BadRequest("Failed to add duty.");
            }
            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAstronautDetails([FromBody] AstronautDetailRequestDto request)
        {
            if (request == null || string.IsNullOrEmpty(request.Name))
            {
                return BadRequest("Invalid request data.");
            }
            bool success = await _astronautDetailService.UpdateAstronautDetailAsync(request);

            if (!success)
            {
                await _logRepository.AddLogAsync(new ProcessLog
                {
                    Level =  "INFO",
                    Message = $"{request.Name} was not updated",
                    Context = "AstronautDetailController.UpdateAstronautDetails"
                });
                return BadRequest("Failed to update detail.");
            }
            await _logRepository.AddLogAsync(new ProcessLog
            {
                Level =  "INFO",
                Message = $"Successfully updated {request.Name} to {request.CurrentDutyTitle} or {request.CurrentRank}",
                Context = "AstronautDetailController.UpdateAstronautDetails"
            });
            return Ok();
        }
        
        [HttpGet("{name}")]
        public async Task<IActionResult> GetDetailByName(string name)
        {
            if (string.IsNullOrEmpty(name)) return BadRequest("There must be a name");

            var detail = await _astronautDetailService.GetDetailByName(name);

            if (detail is null) return BadRequest("This person has no detail");

            return Ok(detail);
        }
    }
}