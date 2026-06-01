using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Application.Readings.Commands;
using SmartHome.Application.Readings.Queries;
using SmartHome.Domain.Entities;
using System.Security.Claims;

namespace SmartHome.API.Controllers
{
    [ApiController]
    [Route("api/readings")]
    [Authorize]
    public class ReadingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReadingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private Guid GetUserId() =>
            Guid.Parse(User.FindFirstValue(
                ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> GetReadings()
        {
            try
            {
                var result = await _mediator.Send(
                    new GetReadingsQuery(GetUserId()));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatest()
        {
            try
            {
                var result = await _mediator.Send(
                    new GetLatestReadingQuery(GetUserId()));

                if (result is null)
                    return NotFound(new
                    {
                        message = "Показаний нет"
                    });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddReading(
            [FromBody] AddReadingCommand command)
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
        //public async Task<IActionResult> DeleteReading(Guid id)
        //{
        //    return Ok(new { message = "Coming soon" });
        //}
    }
}
