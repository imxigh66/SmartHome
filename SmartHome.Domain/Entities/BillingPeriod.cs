using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Domain.Entities
{
    public class BillingPeriod
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid FromReadingId { get; set; }
        public Guid ToReadingId { get; set; }

        public DateOnly PeriodStart { get; set; }
        public DateOnly PeriodEnd { get; set; }

        // Потребление
        public decimal DayConsumption { get; set; }    // кВт·ч день
        public decimal NightConsumption { get; set; }  // кВт·ч ночь
        public decimal TotalConsumption { get; set; }  // кВт·ч итого

        // Сумма в леях
        public decimal DayAmount { get; set; }
        public decimal NightAmount { get; set; }
        public decimal TotalAmount { get; set; }

        // Калибровка
        public decimal ExplainedPercent { get; set; }   // % объяснённости
        public decimal ExplainedAmount { get; set; }    // сумма по приборам
        public decimal CalibrationFactor { get; set; } = 1.0m;

        // Snapshot тарифа на момент расчёта
        public string TariffSnapshot { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public User User { get; set; } = null!;
        public MeterReading FromReading { get; set; } = null!;
        public MeterReading ToReading { get; set; } = null!;
    }
}
