using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DnevnikPrehrane.Models
{
    public class Obrok
    {
        public int ObrokId { get; set; }
        [DisplayName("Datum")]
        public DateTime Date { get; set; }
        [DisplayName("Ime namirnice")]
        public string ImeNamirnice { get; set; }
        [DisplayName("Količina(u gramima)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Količina mora biti veća od 0!")]

        public double Količina { get; set; }
        public double Kalorije { get; set; }
        public double Protein { get; set; }
        public double Ugljikohidrati { get; set; }
        public double Masti { get; set; }

        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }
    }
}
