using Microsoft.AspNetCore.Identity;

namespace DnevnikPrehrane.Models
{
    public class ZapisMase
    {
        public int ZapisMaseId { get; set; }
        public double Masa { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }

    }
}
