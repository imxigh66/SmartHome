using MediatR;

namespace SmartHome.Application.Tariffs.Queries
{
    public record GetMyTariffQuery(
    Guid UserId
) : IRequest<TariffResult>;

    public record TariffResult(
        Guid Id,
        string Provider,
        string PlanType,
        decimal SingleRate,
        decimal DayRate,
        decimal NightRate,
        bool IsManualOverride
    );
}
