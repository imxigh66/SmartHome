using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Common.Interfaces;
using SmartHome.Application.Readings.Commands;
using SmartHome.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Readings.CommandHandler
{
    public class AddReadingCommandHandler : IRequestHandler<AddReadingCommand, AddReadingResult>
    {
        private readonly IAppDbContext _context;

        public AddReadingCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<AddReadingResult> Handle(AddReadingCommand request, CancellationToken cancellationToken)
        {
            var previousReading= await _context.MeterReadings
                .Where(r=>r.UserId == request.UserId)
                .OrderByDescending(r => r.ReadingDate)
                .FirstOrDefaultAsync(cancellationToken);

            if (previousReading != null && request.DayReading < previousReading.DayReading)
            {
                throw new Exception("Day reading cannot be less than previous reading");
            }

            if (request.ReadingDate > DateOnly.FromDateTime(DateTime.Today))
            {
                throw new Exception("Reading date cannot be in the future");
            }

            var reading = new Domain.Entities.MeterReading
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                ReadingDate = request.ReadingDate,
                DayReading = request.DayReading,
                NightReading = request.NightReading,
                IsTwoZone = request.IsTwoZone,
                InputMethod = "MANUAL",
                CreatedAt = DateTime.UtcNow
            };

            await _context.MeterReadings.AddAsync(
            reading, cancellationToken);

            BillingResult? billing = null;

            if(previousReading != null)
            {
                var tariff = await _context.TariffSettings
                .FirstOrDefaultAsync(
                    t => t.UserId == request.UserId,
                    cancellationToken);

                if (tariff != null)
                {
                    billing = CalculateBilling(
                        previousReading, reading, tariff);

                    var period = new BillingPeriod
                    {
                        Id = Guid.NewGuid(),
                        UserId = request.UserId,
                        FromReadingId = previousReading.Id,
                        ToReadingId = reading.Id,
                        PeriodStart = previousReading.ReadingDate,
                        PeriodEnd = reading.ReadingDate,
                        DayConsumption = billing.DayConsumption,
                        NightConsumption = billing.NightConsumption,
                        TotalConsumption = billing.TotalConsumption,
                        DayAmount = billing.DayAmount,
                        NightAmount = billing.NightAmount,
                        TotalAmount = billing.TotalAmount,
                        Recommendation = billing.Recommendation,
                        TariffSnapshot =
                            $"{tariff.Provider}|{tariff.PlanType}|" +
                            $"{tariff.SingleRate}|{tariff.DayRate}|" +
                            $"{tariff.NightRate}",
                        CreatedAt = DateTime.UtcNow
                    };

                    await _context.BillingPeriods.AddAsync(
                        period, cancellationToken);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new AddReadingResult(
                reading.Id,
                reading.ReadingDate,
                reading.DayReading,
                reading.NightReading,
                reading.IsTwoZone,
                billing
            );
        }

        private BillingResult CalculateBilling(
        MeterReading from,
        MeterReading to,
        TariffSettings tariff)
        {
            var dayConsumption = to.DayReading - from.DayReading;
            var nightConsumption = to.NightReading.HasValue &&
                                   from.NightReading.HasValue
                ? to.NightReading.Value - from.NightReading.Value
                : 0;

            var totalConsumption = dayConsumption + nightConsumption;

            decimal dayAmount, nightAmount, totalAmount;

            if (tariff.PlanType == "TWO_ZONE")
            {
                dayAmount = dayConsumption * tariff.DayRate;
                nightAmount = nightConsumption * tariff.NightRate;
                totalAmount = dayAmount + nightAmount;
            }
            else
            {
                dayAmount = totalConsumption * tariff.SingleRate;
                nightAmount = 0;
                totalAmount = dayAmount;
            }

            // Генерируем рекомендацию
            var recommendation = GenerateRecommendation(
                totalConsumption, nightConsumption,
                totalConsumption, tariff);

            return new BillingResult(
                totalConsumption,
                totalAmount,
                dayConsumption,
                nightConsumption,
                dayAmount,
                nightAmount,
                recommendation
            );
        }

        private string GenerateRecommendation(
            decimal totalConsumption,
            decimal nightConsumption,
            decimal total,
            TariffSettings tariff)
        {
            // Единый тариф + большое потребление
            if (tariff.PlanType == "SINGLE_ZONE" &&
                totalConsumption > 100)
            {
                var saving = totalConsumption * 0.3m *
                    (tariff.DayRate - tariff.NightRate);
                return $"Переход на двухзонный тариф сэкономит " +
                       $"~{saving:F0} лей/мес";
            }

            // Мало ночного потребления
            if (tariff.PlanType == "TWO_ZONE" && total > 0 &&
                nightConsumption / total < 0.15m)
            {
                return "Запускай стиральную машину после 23:00 — " +
                       "ночной тариф дешевле на 15%";
            }

            // Высокое потребление
            if (totalConsumption > 200)
            {
                return "Высокое потребление. Проверь бойлер — " +
                       "обычно это главный потребитель";
            }

            return "Потребление в норме. Продолжай следить " +
                   "за показаниями каждый месяц";
        }
    }
}
