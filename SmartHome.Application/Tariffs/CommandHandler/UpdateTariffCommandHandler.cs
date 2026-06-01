using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Common.Interfaces;
using SmartHome.Application.Tariffs.Commands;
using SmartHome.Application.Tariffs.Queries;

namespace SmartHome.Application.Tariffs.Commands;
public class UpdateTariffCommandHandler : IRequestHandler<UpdateTariffCommand, TariffResult>
{
    private readonly IAppDbContext _context;

    public UpdateTariffCommandHandler(IAppDbContext context)
    {
        _context = context;
    }
    public async Task<TariffResult> Handle(UpdateTariffCommand request, CancellationToken cancellationToken)
    {
        var tariff = await _context.TariffSettings
            .FirstOrDefaultAsync(
                t => t.UserId == request.UserId,
                cancellationToken);

        if (tariff is null)
            throw new Exception("Tariff settings not found.");

        tariff.Provider = request.Provider;
        tariff.PlanType = request.PlanType;
        tariff.SingleRate = request.SingleRate;
        tariff.DayRate = request.DayRate;
        tariff.NightRate = request.NightRate;
        tariff.IsManualOverride = true;
        tariff.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new TariffResult(
            tariff.Id,
            tariff.Provider,
            tariff.PlanType,
            tariff.SingleRate,
            tariff.DayRate,
            tariff.NightRate,
            tariff.IsManualOverride
        );
    }
}

