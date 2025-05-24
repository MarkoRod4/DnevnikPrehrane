using Microsoft.AspNetCore.Identity;

namespace DnevnikPrehrane.Models
{
    public class Biljeska
    {
        public int BiljeskaId { get; set; }
        public string Tekst { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }
    }
}
