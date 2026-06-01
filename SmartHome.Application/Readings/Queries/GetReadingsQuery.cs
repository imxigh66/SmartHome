using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Readings.Queries
{
    public record GetReadingsQuery(
    Guid UserId
) : IRequest<List<ReadingResult>>;

    public record ReadingResult(
        Guid Id,
        DateOnly ReadingDate,
        decimal DayReading,
        decimal? NightReading,
        bool IsTwoZone,
        string InputMethod,
        DateTime CreatedAt
    );
}
