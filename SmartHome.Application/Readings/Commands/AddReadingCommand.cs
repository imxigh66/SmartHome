using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SmartHome.Application.Readings.Commands
{
    public record AddReadingCommand(
    DateOnly ReadingDate,
    decimal DayReading,
    decimal? NightReading,
    bool IsTwoZone
) : IRequest<AddReadingResult>
    {
        [JsonIgnore]
        public Guid UserId { get; init; }
    };

    public record AddReadingResult(
        Guid Id,
        DateOnly ReadingDate,
        decimal DayReading,
        decimal? NightReading,
        bool IsTwoZone,
        BillingResult? Billing
    );

    public record BillingResult(
        decimal TotalConsumption,
        decimal TotalAmount,
        decimal DayConsumption,
        decimal NightConsumption,
        decimal DayAmount,
        decimal NightAmount,
        string Recommendation
    );
}
