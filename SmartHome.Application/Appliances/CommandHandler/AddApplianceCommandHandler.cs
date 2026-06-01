using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Appliances.Commands;
using SmartHome.Application.Common.Interfaces;
using SmartHome.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Appliances.CommandHandler
{
    public class AddApplianceCommandHandler : IRequestHandler<AddApplianceCommand, ApplianceResult>
    {
        private readonly IAppDbContext _context;

        public AddApplianceCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<ApplianceResult> Handle(AddApplianceCommand request, CancellationToken cancellationToken)
        {
            var tariff = await _context.TariffSettings
            .FirstOrDefaultAsync(
                t => t.UserId == request.UserId,
                cancellationToken);

            var rate = tariff?.SingleRate ?? 3.59m;

            // Считаем стоимость в месяц
            var monthlyCost = request.WattTypical / 1000m
                * request.HoursPerDay
                * 30
                * rate;

            // Генерируем совет по прибору
            var tip = GenerateTip(request.Name,
                request.WattTypical, request.HoursPerDay);

            var appliance = new Appliance
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Name = request.Name,
                Icon = request.Icon,
                WattTypical = request.WattTypical,
                HoursPerDay = request.HoursPerDay,
                CalibratedHoursPerDay = request.HoursPerDay,
                CalibrationFactor = 1.0m,
                Tip = tip,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Appliances.AddAsync(
                appliance, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new ApplianceResult(
                appliance.Id,
                appliance.Name,
                appliance.Icon,
                appliance.WattTypical,
                appliance.HoursPerDay,
                monthlyCost,
                tip
            );
        }

        private string? GenerateTip(
        string name, decimal watt, decimal hours)
        {
            var nameLower = name.ToLower();

            if (nameLower.Contains("бойлер") ||
                nameLower.Contains("boiler"))
                return "Снизь температуру с 75°C до 60°C " +
                       "— экономия до 20%";

            if (nameLower.Contains("стирал") ||
                nameLower.Contains("washing"))
                return "Запускай после 23:00 — ночной тариф " +
                       "дешевле на 15%";

            if (nameLower.Contains("сушил") ||
                nameLower.Contains("dryer"))
                return "Замени сушилку на сушку на воздухе " +
                       "— экономия ~200 лей/мес";

            if (nameLower.Contains("холодил") ||
                nameLower.Contains("fridge") && watt > 200)
                return "Старый холодильник потребляет в 3 раза " +
                       "больше нового. Замена окупится за 2 года";

            if (nameLower.Contains("обогрев") ||
                nameLower.Contains("heater"))
                return $"Обогреватель на {watt}Вт за {hours}ч " +
                       $"= {watt / 1000 * hours * 3.59m:F0} лей/день";

            if (watt > 2000)
                return "Мощный прибор — следи чтобы не работал " +
                       "вхолостую";

            return null;
        }
    }
}
