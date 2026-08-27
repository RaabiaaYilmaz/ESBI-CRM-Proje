using CRMProject.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRMProject.Data.Context
{
    // Uygulamanın veritabanı bağlamı — Identity + kendi tablolarımız burada birleşiyor
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Cari> Cariler { get; set; }
        public DbSet<Malzeme> Malzemeler { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Yalnızca aktif kayıtlar arasında benzersizlik aranır — silinen (pasif) kayıtların kodu tekrar kullanılabilir
            builder.Entity<Cari>()
                .HasIndex(c => c.CariKodu)
                .IsUnique()
                .HasFilter("[AktifMi] = 1");

            builder.Entity<Malzeme>()
                .HasIndex(m => m.MalzemeKodu)
                .IsUnique()
                .HasFilter("[AktifMi] = 1");

            // Decimal alanlarda hassasiyet belirtmezsek EF Core uyarı verir
            builder.Entity<Malzeme>()
                .Property(m => m.SatisFiyati).HasColumnType("decimal(18,2)");
            builder.Entity<Malzeme>()
                .Property(m => m.AlisFiyati).HasColumnType("decimal(18,2)");
            builder.Entity<Malzeme>()
                .Property(m => m.StokMiktari).HasColumnType("decimal(18,2)");
            builder.Entity<Malzeme>()
                .Property(m => m.KritikStok).HasColumnType("decimal(18,2)");
        }
    }
}