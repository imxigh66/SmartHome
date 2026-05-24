using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Billing.Queries
{
    public record GetBillingPeriodsQuery(
    Guid UserId
) : IRequest<List<BillingPeriodResult>>;

    public record BillingPeriodResult(
        Guid Id,
        DateOnly PeriodStart,
        DateOnly PeriodEnd,
        decimal TotalConsumption,
        decimal TotalAmount,
        decimal DayConsumption,
        decimal NightConsumption,
        decimal DayAmount,
        decimal NightAmount,
        DateTime CreatedAt
    );
}
