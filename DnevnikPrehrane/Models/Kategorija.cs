using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace DnevnikPrehrane.Models
{
    public class Kategorija
    {
        public int KategorijaId { get; set; }
        [DisplayName("Ime kategorije")]
        public string Name { get; set; }
        public string? UserId { get; set; }
        public virtual IdentityUser? User { get; set; }
        public virtual ICollection<Namirnica>? Namirnice { get; set; }
    }
}
