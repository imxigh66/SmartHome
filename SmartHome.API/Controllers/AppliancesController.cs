using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Application.Appliances.Commands;
using SmartHome.Application.Appliances.Queries;
using SmartHome.Domain.Entities;
using System.Security.Claims;

namespace SmartHome.API.Controllers
{

    [ApiController]
    [Route("api/appliances")]
    [Authorize]
    public class AppliancesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppliancesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private Guid GetUserId() =>
            Guid.Parse(User.FindFirstValue(
                ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> GetAppliances()
        {
            try
            {
                var result = await _mediator.Send(
                    new GetAppliancesQuery(GetUserId()));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAppliance(
            [FromBody] AddApplianceCommand command)
        {
            try
            {
                var commandWithUser = command with
                {
                    UserId = GetUserId()
                };
                var result = await _mediator.Send(commandWithUser);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteAppliance(Guid id)
        //{
        //    return Ok(new { message = "Coming soon" });
        //}
    }
}
