using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Readings.Queries
{
    public record GetLatestReadingQuery(
    Guid UserId
) : IRequest<ReadingResult?>;
}
