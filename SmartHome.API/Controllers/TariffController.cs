using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Application.Tariffs.Commands;
using SmartHome.Application.Tariffs.Queries;
using SmartHome.Domain.Entities;
using System.Security.Claims;

namespace SmartHome.API.Controllers
{
    [ApiController]
    [Route("api/tariffs")]
    [Authorize]
    public class TariffController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TariffController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private Guid GetUserId() =>
            Guid.Parse(User.FindFirstValue(
                ClaimTypes.NameIdentifier)!);

        [HttpGet("my")]
        public async Task<IActionResult> GetMyTariff()
        {
            try
            {
                var result = await _mediator.Send(
                    new GetMyTariffQuery(GetUserId()));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("my")]
        public async Task<IActionResult> UpdateMyTariff(
            [FromBody] UpdateTariffRequest request)
        {
            try
            {
                var result = await _mediator.Send(
                    new UpdateTariffCommand(
                        GetUserId(),
                        request.Provider,
                        request.PlanType,
                        request.SingleRate,
                        request.DayRate,
                        request.NightRate));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("global")]
        [AllowAnonymous]
        public IActionResult GetGlobalTariffs()
        {
            var tariffs = new[]
            {
            new
            {
                Provider = "PREMIER_ENERGY",
                PlanType = "SINGLE_ZONE",
                Rate = 3.59m,
                Zone = "SINGLE",
                ValidFrom = "2025-08-01"
            },
            new
            {
                Provider = "PREMIER_ENERGY",
                PlanType = "TWO_ZONE",
                Rate = 3.89m,
                Zone = "DAY",
                ValidFrom = "2025-08-01"
            },
            new
            {
                Provider = "PREMIER_ENERGY",
                PlanType = "TWO_ZONE",
                Rate = 3.31m,
                Zone = "NIGHT",
                ValidFrom = "2025-08-01"
            },
            new
            {
                Provider = "FEE_NORD",
                PlanType = "SINGLE_ZONE",
                Rate = 4.00m,
                Zone = "SINGLE",
                ValidFrom = "2025-08-01"
            },
            new
            {
                Provider = "FEE_NORD",
                PlanType = "TWO_ZONE",
                Rate = 4.26m,
                Zone = "DAY",
                ValidFrom = "2025-08-01"
            },
            new
            {
                Provider = "FEE_NORD",
                PlanType = "TWO_ZONE",
                Rate = 3.76m,
                Zone = "NIGHT",
                ValidFrom = "2025-08-01"
            }
        };

            return Ok(tariffs);
        }
    }

    public record UpdateTariffRequest(
        string Provider,
        string PlanType,
        decimal SingleRate,
        decimal DayRate,
        decimal NightRate
    );
}
