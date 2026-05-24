using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Appliances.Commands
{
    public record AddApplianceCommand(
    string Name,
    string Icon,
    decimal WattTypical,
    decimal HoursPerDay
) : IRequest<ApplianceResult>
    {
        public Guid UserId { get; init; }
    }

    public record ApplianceResult(
        Guid Id,
        string Name,
        string Icon,
        decimal WattTypical,
        decimal HoursPerDay,
        decimal MonthlyCostLei,
        string? Tip
    );
}
