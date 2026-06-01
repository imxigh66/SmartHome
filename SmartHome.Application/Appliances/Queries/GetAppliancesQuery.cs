using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Appliances.Queries
{
    public record GetAppliancesQuery(
    Guid UserId
) : IRequest<List<ApplianceStatsResult>>;

    public record ApplianceStatsResult(
        Guid Id,
        string Name,
        string Icon,
        decimal WattTypical,
        decimal HoursPerDay,
        decimal MonthlyCostLei,
        decimal PercentOfBill,
        string? Tip
    );
}
