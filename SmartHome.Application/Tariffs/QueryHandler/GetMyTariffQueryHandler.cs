using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Common.Interfaces;
using SmartHome.Application.Tariffs.Queries;

namespace SmartHome.Application.Tariffs.QueryHandler
{
    public class GetMyTariffQueryHandler : IRequestHandler<GetMyTariffQuery, TariffResult>
    {
        private readonly IAppDbContext _context;

        public GetMyTariffQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<TariffResult> Handle(GetMyTariffQuery request, CancellationToken cancellationToken)
        {
            var tariff = await _context.TariffSettings.FirstOrDefaultAsync(t => t.UserId == request.UserId, cancellationToken);

            if (tariff == null) {
                throw new Exception("Tariff settings not found for the user.");
            }

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
}
