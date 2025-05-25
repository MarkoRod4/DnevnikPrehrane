using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DnevnikPrehrane.Models
{
    public class ZapisMase
    {
        public int ZapisMaseId { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Masa mora biti veća od 0!")]
        public double Masa { get; set; }
        [DisplayName("Datum")]
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }

    }
}
