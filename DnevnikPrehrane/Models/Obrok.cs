using Microsoft.AspNetCore.Identity;

namespace DnevnikPrehrane.Models
{
    public class Obrok
    {
        public int ObrokId { get; set; }
        public DateTime Date { get; set; }
        public string ImeNamirnice { get; set; }
        public double Količina { get; set; }
        public double Kalorije { get; set; }
        public double Protein { get; set; }
        public double Ugljikohidrati { get; set; }
        public double Masti { get; set; }

        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }
    }
}
