using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace DnevnikPrehrane.Models
{
    public class Biljeska
    {
        public int BiljeskaId { get; set; }
        public string Tekst { get; set; }

        [DisplayName("Datum")]
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }
    }
}
