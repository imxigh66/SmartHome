using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Application.Dashboard.Queries;
using SmartHome.Domain.Entities;
using System.Security.Claims;

namespace SmartHome.API.Controllers
{

    [ApiController]
    [Route("api/dashboard")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private Guid GetUserId() =>
            Guid.Parse(User.FindFirstValue(
                ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
                var result = await _mediator.Send(
                    new GetDashboardQuery(GetUserId()));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
