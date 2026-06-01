using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Common.Interfaces;
using SmartHome.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Infrastructure.Database
{
    public class AppDbContext:DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions options)
        : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<TariffSettings> TariffSettings { get; set; }
        public DbSet<MeterReading> MeterReadings { get; set; }
        public DbSet<BillingPeriod> BillingPeriods { get; set; }
        public DbSet<Appliance> Appliances { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(u => u.Id);
                e.HasIndex(u => u.Email).IsUnique();
                e.Property(u => u.Email).HasMaxLength(255);
                e.Property(u => u.Name).HasMaxLength(100);
                e.Property(u => u.PasswordHash).HasMaxLength(255);
            });

            // TariffSettings
            modelBuilder.Entity<TariffSettings>(e =>
            {
                e.HasKey(t => t.Id);
                e.Property(t => t.SingleRate).HasPrecision(6, 4);
                e.Property(t => t.DayRate).HasPrecision(6, 4);
                e.Property(t => t.NightRate).HasPrecision(6, 4);
                e.HasOne(t => t.User)
                 .WithOne(u => u.TariffSettings)
                 .HasForeignKey<TariffSettings>(t => t.UserId);
            });

            // MeterReading
            modelBuilder.Entity<MeterReading>(e =>
            {
                e.HasKey(m => m.Id);
                e.Property(m => m.DayReading).HasPrecision(10, 1);
                e.Property(m => m.NightReading).HasPrecision(10, 1);
                e.HasOne(m => m.User)
                 .WithMany(u => u.MeterReadings)
                 .HasForeignKey(m => m.UserId);
            });

            // BillingPeriod
            modelBuilder.Entity<BillingPeriod>(e =>
            {
                e.HasKey(b => b.Id);
                e.Property(b => b.TotalAmount).HasPrecision(10, 2);
                e.Property(b => b.TotalConsumption).HasPrecision(10, 2);
                e.Property(b => b.CalibrationFactor).HasPrecision(6, 4);
                e.HasOne(b => b.User)
                 .WithMany()
                 .HasForeignKey(b => b.UserId);
                e.HasOne(b => b.FromReading)
                 .WithMany(m => m.BillingPeriodsFrom)
                 .HasForeignKey(b => b.FromReadingId)
                 .OnDelete(DeleteBehavior.NoAction);
                e.HasOne(b => b.ToReading)
                 .WithMany(m => m.BillingPeriodsTo)
                 .HasForeignKey(b => b.ToReadingId)
                 .OnDelete(DeleteBehavior.NoAction);
            });

            // Appliance
            modelBuilder.Entity<Appliance>(e =>
            {
                e.HasKey(a => a.Id);
                e.Property(a => a.WattTypical).HasPrecision(8, 1);
                e.Property(a => a.HoursPerDay).HasPrecision(4, 1);
                e.HasOne(a => a.User)
                 .WithMany(u => u.Appliances)
                 .HasForeignKey(a => a.UserId);
            });
        }
    }
}
