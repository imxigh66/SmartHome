using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Common.Interfaces;
using SmartHome.Application.Readings.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Readings.QueryHandler
{
    class GetLatestReadingQueryHandler : IRequestHandler<GetLatestReadingQuery, ReadingResult?>
    {
        private readonly IAppDbContext _context;

        public GetLatestReadingQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<ReadingResult?> Handle(GetLatestReadingQuery request, CancellationToken cancellationToken)
        {
            var reading = await _context.MeterReadings
            .Where(r => r.UserId == request.UserId)
            .OrderByDescending(r => r.ReadingDate)
            .FirstOrDefaultAsync(cancellationToken);

            if (reading is null) return null;

            return new ReadingResult(
                reading.Id,
                reading.ReadingDate,
                reading.DayReading,
                reading.NightReading,
                reading.IsTwoZone,
                reading.InputMethod,
                reading.CreatedAt
            );
        }
    }
}
