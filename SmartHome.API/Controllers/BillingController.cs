using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Application.Billing.Queries;
using SmartHome.Domain.Entities;
using System.Security.Claims;

namespace SmartHome.API.Controllers
{
    [ApiController]
    [Route("api/billing")]
    [Authorize]
    public class BillingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BillingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private Guid GetUserId() =>
            Guid.Parse(User.FindFirstValue(
                ClaimTypes.NameIdentifier)!);

        [HttpGet("periods")]
        public async Task<IActionResult> GetPeriods()
        {
            try
            {
                var result = await _mediator.Send(
                    new GetBillingPeriodsQuery(GetUserId()));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("current-month")]
        public async Task<IActionResult> GetCurrentMonth()
        {
            try
            {
                var result = await _mediator.Send(
                    new GetCurrentMonthForecastQuery(GetUserId()));

                if (result is null)
                    return NotFound(new
                    {
                        message = "Нет данных за текущий месяц"
                    });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
