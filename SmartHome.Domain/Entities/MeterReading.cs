using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Domain.Entities
{
    public class MeterReading
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public DateOnly ReadingDate { get; set; }
        public decimal DayReading { get; set; }     
        public decimal? NightReading { get; set; }  
        public bool IsTwoZone { get; set; } = false;

        public string InputMethod { get; set; } = "MANUAL";
        // MANUAL / OCR / AUTO

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public User User { get; set; } = null!;
        public ICollection<BillingPeriod> BillingPeriodsFrom { get; set; } = new List<BillingPeriod>();
        public ICollection<BillingPeriod> BillingPeriodsTo { get; set; } = new List<BillingPeriod>();
    }
}
