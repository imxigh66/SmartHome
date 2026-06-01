using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Appliances.Queries;
using SmartHome.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Appliances.QueryHandler
{
    public class GetAppliancesQueryHandler : IRequestHandler<GetAppliancesQuery, List<ApplianceStatsResult>>
    {
        private readonly IAppDbContext _context;

        public GetAppliancesQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ApplianceStatsResult>> Handle(GetAppliancesQuery request, CancellationToken cancellationToken)
        {
            var tariff = await _context.TariffSettings
            .FirstOrDefaultAsync(
                t => t.UserId == request.UserId,
                cancellationToken);

            var rate = tariff?.SingleRate ?? 3.59m;

            var appliances = await _context.Appliances
                .Where(a => a.UserId == request.UserId)
                .ToListAsync(cancellationToken);

           
            var results = appliances.Select(a =>
            {
                var cost = a.WattTypical / 1000m
                    * a.HoursPerDay * 30 * rate;
                return (appliance: a, cost);
            }).ToList();

            var totalCost = results.Sum(r => r.cost);

            return results
                .OrderByDescending(r => r.cost)
                .Select(r => new ApplianceStatsResult(
                    r.appliance.Id,
                    r.appliance.Name,
                    r.appliance.Icon,
                    r.appliance.WattTypical,
                    r.appliance.HoursPerDay,
                    r.cost,
                    totalCost > 0
                        ? Math.Round(r.cost / totalCost * 100, 1)
                        : 0,
                    r.appliance.Tip
                ))
                .ToList();
        }
    }
}
