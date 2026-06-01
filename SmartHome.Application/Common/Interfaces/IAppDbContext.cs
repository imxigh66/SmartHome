using Microsoft.EntityFrameworkCore;
using SmartHome.Domain.Entities;
using System.Collections.Generic;

namespace SmartHome.Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; }
        DbSet<TariffSettings> TariffSettings { get; }
        DbSet<MeterReading> MeterReadings { get; }
        DbSet<BillingPeriod> BillingPeriods { get; }
        DbSet<Appliance> Appliances { get; }

        Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default);
    }
}
