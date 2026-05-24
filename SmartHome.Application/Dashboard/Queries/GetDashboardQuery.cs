using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Dashboard.Queries
{
    public record GetDashboardQuery(
    Guid UserId
) : IRequest<DashboardResult>;

    public record DashboardResult(
        // Прогноз месяца
        decimal? ForecastTotal,
        decimal? MonthAmount,
        decimal? MonthConsumption,
        int DaysElapsed,
        int DaysRemaining,

        // Последнее показание
        DateOnly? LastReadingDate,
        decimal? LastReadingValue,

        // Тариф
        string Provider,
        string PlanType,
        decimal CurrentRate,

        // Рекомендация
        string? Recommendation,

        // Состояние
        bool HasReadings
    );
}
