using MediatR;
using SmartHome.Application.Tariffs.Queries;

namespace SmartHome.Application.Tariffs.Commands
{
    public record UpdateTariffCommand(
    Guid UserId,
    string Provider,
    string PlanType,
    decimal SingleRate,
    decimal DayRate,
    decimal NightRate
) : IRequest<TariffResult>;
}
