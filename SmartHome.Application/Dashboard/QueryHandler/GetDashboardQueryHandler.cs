using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Common.Interfaces;
using SmartHome.Application.Dashboard.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Dashboard.QueryHandler
{
    public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, DashboardResult>
    {
        private readonly IAppDbContext _context;

        public GetDashboardQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<DashboardResult> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var firstDay = new DateOnly(today.Year, today.Month, 1);
            var daysInMonth = DateTime.DaysInMonth(
                today.Year, today.Month);

            // Последнее показание
            var lastReading = await _context.MeterReadings
                .Where(r => r.UserId == request.UserId)
                .OrderByDescending(r => r.ReadingDate)
                .FirstOrDefaultAsync(cancellationToken);

            // Тариф
            var tariff = await _context.TariffSettings
                .FirstOrDefaultAsync(
                    t => t.UserId == request.UserId,
                    cancellationToken);

            // Нет данных
            if (lastReading is null || tariff is null)
            {
                return new DashboardResult(
                    null, null, null, 0, 0,
                    null, null,
                    "PREMIER_ENERGY", "SINGLE_ZONE", 3.59m,
                    null, false
                );
            }

            // Периоды текущего месяца
            var monthPeriods = await _context.BillingPeriods
                .Where(b => b.UserId == request.UserId &&
                            b.PeriodStart >= firstDay)
                .ToListAsync(cancellationToken);

            var monthAmount = monthPeriods.Sum(p => p.TotalAmount);
            var monthConsumption = monthPeriods
                .Sum(p => p.TotalConsumption);

            // Прогноз
            var daysElapsed = today.Day;
            var daysRemaining = daysInMonth - daysElapsed;
            var dailyAverage = daysElapsed > 0
                ? monthAmount / daysElapsed : 0;
            var forecastTotal = monthAmount +
                dailyAverage * daysRemaining;

            // Последняя рекомендация
            var lastPeriod = await _context.BillingPeriods
                .Where(b => b.UserId == request.UserId)
                .OrderByDescending(b => b.PeriodEnd)
                .FirstOrDefaultAsync(cancellationToken);

            var currentRate = tariff.PlanType == "TWO_ZONE"
                ? tariff.DayRate
                : tariff.SingleRate;

            return new DashboardResult(
                forecastTotal,
                monthAmount,
                monthConsumption,
                daysElapsed,
                daysRemaining,
                lastReading.ReadingDate,
                lastReading.DayReading,
                tariff.Provider,
                tariff.PlanType,
                currentRate,
                lastPeriod?.Recommendation,
                true
            );
        }
    }
}
