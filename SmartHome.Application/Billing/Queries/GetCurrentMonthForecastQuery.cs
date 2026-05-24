using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Billing.Queries
{
    public record GetCurrentMonthForecastQuery(
    Guid UserId
) : IRequest<ForecastResult?>;

    public record ForecastResult(
        decimal MonthConsumption,
        decimal MonthAmount,
        decimal ForecastTotal,
        decimal DailyAverage,
        int DaysElapsed,
        int DaysRemaining
    );
}
