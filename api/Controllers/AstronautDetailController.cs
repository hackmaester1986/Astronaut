using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stargate.Services;
using StargateAPI.Business.Commands;
using StargateAPI.Services;
using System.Net;

namespace StargateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AstronautDetailController : ControllerBase
    {
        private readonly IAstronautDetailService _astronautDetailService;
        public AstronautDetailController(IAstronautDetailService astronautDetailService)
        {
            _astronautDetailService = astronautDetailService;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAstronautDetail([FromBody] CreateAstronautDetailDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Rank) || string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest("Invalid request data.");
            }
            bool success = await _astronautDetailService.CreateAstronautDetail(request.Name, request.Rank, request.Title);
            if (!success)
            {
                return BadRequest("Failed to add duty.");
            }
            return Ok();
        }
    }
}