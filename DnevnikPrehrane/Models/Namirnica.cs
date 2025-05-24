using Microsoft.AspNetCore.Identity;

namespace DnevnikPrehrane.Models
{
    public class Namirnica
    {
        public int NamirnicaId { get; set; }
        public string Name { get; set; }

        public string? UserId { get; set; }
        public virtual IdentityUser? User { get; set; }
        public int KategorijaId { get; set; }
        public virtual Kategorija Kategorija { get; set; }
        public double Kalorije { get; set; }
        public double Protein { get; set; }
        public double Ugljikohidrati { get; set; }
        public double Masti { get; set; }

    }
}
