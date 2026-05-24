using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Domain.Entities
{
    public class Appliance
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = "🔌";

        // Мощность
        public decimal WattMin { get; set; }
        public decimal WattMax { get; set; }
        public decimal WattTypical { get; set; }

        // Использование
        public decimal HoursPerDay { get; set; }
        public decimal CalibratedHoursPerDay { get; set; }
        public decimal CalibrationFactor { get; set; } = 1.0m;
        public DateTime? CalibrationDate { get; set; }

        // Совет по прибору
        public string? Tip { get; set; }

        // Связь с Shelly устройством
        public Guid? ShellyDeviceId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public User User { get; set; } = null!;
        //public ShellyDevice? ShellyDevice { get; set; }
    }
}
