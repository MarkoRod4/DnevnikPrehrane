using DnevnikPrehrane.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DnevnikPrehrane.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Namirnica> Namirnice { get; set; }
        public DbSet<Obrok> Obroci { get; set; }
        public DbSet<Kategorija> Kategorije { get; set; }
        public DbSet<Biljeska> Biljeske { get; set; }
        public DbSet<ZapisMase> ZapisiMase { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var kategorije = new List<Kategorija>
            {
                new() { KategorijaId = 1, Name = "Voće", UserId = null },
                new() { KategorijaId = 2, Name = "Povrće", UserId = null },
                new() { KategorijaId = 3, Name = "Meso", UserId = null },
                new() { KategorijaId = 4, Name = "Riba", UserId = null },
                new() { KategorijaId = 5, Name = "Žitarice", UserId = null },
                new() { KategorijaId = 6, Name = "Mliječni proizvodi", UserId = null },
                new() { KategorijaId = 7, Name = "Ostalo", UserId = null },
            };

            builder.Entity<Kategorija>().HasData(kategorije);

            var namirnice = new List<Namirnica>
            {
                // Voće
                new() { NamirnicaId = 1, Name = "Jabuka", KategorijaId = 1, Kalorije = 52, Protein = 0.3, Ugljikohidrati = 14, Masti = 0.2, UserId = null },
                new() { NamirnicaId = 2, Name = "Banana", KategorijaId = 1, Kalorije = 89, Protein = 1.1, Ugljikohidrati = 23, Masti = 0.3, UserId = null },
                new() { NamirnicaId = 3, Name = "Naranča", KategorijaId = 1, Kalorije = 47, Protein = 0.9, Ugljikohidrati = 12, Masti = 0.1, UserId = null },

                // Povrće
                new() { NamirnicaId = 4, Name = "Mrkva", KategorijaId = 2, Kalorije = 41, Protein = 0.9, Ugljikohidrati = 10, Masti = 0.2, UserId = null },
                new() { NamirnicaId = 5, Name = "Brokula", KategorijaId = 2, Kalorije = 34, Protein = 2.8, Ugljikohidrati = 7, Masti = 0.4, UserId = null },
                new() { NamirnicaId = 6, Name = "Rajčica", KategorijaId = 2, Kalorije = 18, Protein = 0.9, Ugljikohidrati = 3.9, Masti = 0.2, UserId = null },

                // Meso
                new() { NamirnicaId = 7, Name = "Pileća prsa", KategorijaId = 3, Kalorije = 165, Protein = 31, Ugljikohidrati = 0, Masti = 3.6, UserId = null },
                new() { NamirnicaId = 8, Name = "Govedina", KategorijaId = 3, Kalorije = 250, Protein = 26, Ugljikohidrati = 0, Masti = 17, UserId = null },

                // Riba
                new() { NamirnicaId = 9, Name = "Tuna", KategorijaId = 4, Kalorije = 132, Protein = 28, Ugljikohidrati = 0, Masti = 1.3, UserId = null },
                new() { NamirnicaId = 10, Name = "Losos", KategorijaId = 4, Kalorije = 208, Protein = 20, Ugljikohidrati = 0, Masti = 13, UserId = null },

                // Žitarice
                new() { NamirnicaId = 11, Name = "Kruh", KategorijaId = 5, Kalorije = 265, Protein = 9, Ugljikohidrati = 49, Masti = 3.2, UserId = null },
                new() { NamirnicaId = 12, Name = "Tjestenina", KategorijaId = 5, Kalorije = 131, Protein = 5, Ugljikohidrati = 25, Masti = 1.1, UserId = null },

                // Mliječni proizvodi
                new() { NamirnicaId = 13, Name = "Mlijeko", KategorijaId = 6, Kalorije = 42, Protein = 3.4, Ugljikohidrati = 5, Masti = 1, UserId = null },
                new() { NamirnicaId = 14, Name = "Sir", KategorijaId = 6, Kalorije = 402, Protein = 25, Ugljikohidrati = 1.3, Masti = 33, UserId = null },
                new() { NamirnicaId = 15, Name = "Jogurt", KategorijaId = 6, Kalorije = 59, Protein = 10, Ugljikohidrati = 3.6, Masti = 0.4, UserId = null },


                // Ostalo
                new() { NamirnicaId = 16, Name = "Jaje", KategorijaId = 7, Kalorije = 155, Protein = 13, Ugljikohidrati = 1.1, Masti = 11, UserId = null },
                new() { NamirnicaId = 17, Name = "Maslinovo ulje", KategorijaId = 7, Kalorije = 884, Protein = 0, Ugljikohidrati = 0, Masti = 100, UserId = null },
                new() { NamirnicaId = 18, Name = "Šećer", KategorijaId = 7, Kalorije = 387, Protein = 0, Ugljikohidrati = 100, Masti = 0, UserId = null }
            };

            builder.Entity<Namirnica>().HasData(namirnice);
        }

    }
}
