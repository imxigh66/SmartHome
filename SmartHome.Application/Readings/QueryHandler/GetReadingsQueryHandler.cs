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
    public class GetReadingsQueryHandler : IRequestHandler<GetReadingsQuery, List<ReadingResult>>
    {
        private readonly IAppDbContext _context;

        public GetReadingsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ReadingResult>> Handle(GetReadingsQuery request, CancellationToken cancellationToken)
        {
            return await _context.MeterReadings
            .Where(r => r.UserId == request.UserId)
            .OrderByDescending(r => r.ReadingDate)
            .Select(r => new ReadingResult(
                r.Id,
                r.ReadingDate,
                r.DayReading,
                r.NightReading,
                r.IsTwoZone,
                r.InputMethod,
                r.CreatedAt
            ))
            .ToListAsync(cancellationToken);
        }
    }
}
