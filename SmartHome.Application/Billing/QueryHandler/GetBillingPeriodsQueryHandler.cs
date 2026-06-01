using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Billing.Queries;
using SmartHome.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Billing.QueryHandler
{
    public class GetBillingPeriodsQueryHandler : IRequestHandler<GetBillingPeriodsQuery, List<BillingPeriodResult>>
    {
        private readonly IAppDbContext _context;

        public GetBillingPeriodsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BillingPeriodResult>> Handle(GetBillingPeriodsQuery request, CancellationToken cancellationToken)
        {
            return await _context.BillingPeriods
            .Where(b => b.UserId == request.UserId)
            .OrderByDescending(b => b.PeriodEnd)
            .Select(b => new BillingPeriodResult(
                b.Id,
                b.PeriodStart,
                b.PeriodEnd,
                b.TotalConsumption,
                b.TotalAmount,
                b.DayConsumption,
                b.NightConsumption,
                b.DayAmount,
                b.NightAmount,
                b.CreatedAt
            ))
            .ToListAsync(cancellationToken);
        }
    }
}
