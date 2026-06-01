using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Domain.Entities
{
    public class TariffSettings
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string Provider { get; set; } = "PREMIER_ENERGY";
        // PREMIER_ENERGY / FEE_NORD

        public string PlanType { get; set; } = "SINGLE_ZONE";
        // SINGLE_ZONE / TWO_ZONE

        public decimal SingleRate { get; set; } = 3.59m;
        public decimal DayRate { get; set; } = 3.89m;
        public decimal NightRate { get; set; } = 3.31m;

        public bool IsManualOverride { get; set; } = false;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

     
        public User User { get; set; } = null!;
    }
}
