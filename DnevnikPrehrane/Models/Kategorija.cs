using Microsoft.AspNetCore.Identity;

namespace DnevnikPrehrane.Models
{
    public class Kategorija
    {
        public int KategorijaId { get; set; }
        public string Name { get; set; }
        public string? UserId { get; set; }
        public virtual IdentityUser? User { get; set; }
        public virtual List<Namirnica>? Namirnice { get; set; }
    }
}
