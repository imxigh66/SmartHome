using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public TariffSettings? TariffSettings { get; set; }
        public ICollection<MeterReading> MeterReadings { get; set; } = new List<MeterReading>();
        public ICollection<Appliance> Appliances { get; set; } = new List<Appliance>();

    }
}
