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
    public class GetCurrentMonthForecastQueryHandler : IRequestHandler<GetCurrentMonthForecastQuery, ForecastResult?>
    {
        private readonly IAppDbContext _context;

        public GetCurrentMonthForecastQueryHandler(
            IAppDbContext context)
        {
            _context = context;
        }
        public async Task<ForecastResult?> Handle(GetCurrentMonthForecastQuery request, CancellationToken cancellationToken)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var firstDay = new DateOnly(today.Year, today.Month, 1);
            var daysInMonth = DateTime.DaysInMonth(
                today.Year, today.Month);
            var daysElapsed = today.Day;
            var daysRemaining = daysInMonth - daysElapsed;

            // Берём периоды текущего месяца
            var periods = await _context.BillingPeriods
                .Where(b => b.UserId == request.UserId &&
                            b.PeriodStart >= firstDay &&
                            b.PeriodEnd <= today)
                .ToListAsync(cancellationToken);

            if (!periods.Any()) return null;

            var monthConsumption = periods
                .Sum(p => p.TotalConsumption);
            var monthAmount = periods
                .Sum(p => p.TotalAmount);

            // Среднесуточное потребление
            var dailyAverage = daysElapsed > 0
                ? monthAmount / daysElapsed
                : 0;

            // Прогноз до конца месяца
            var forecastTotal = monthAmount +
                dailyAverage * daysRemaining;

            return new ForecastResult(
                monthConsumption,
                monthAmount,
                forecastTotal,
                dailyAverage,
                daysElapsed,
                daysRemaining
            );
        }
    }
}
