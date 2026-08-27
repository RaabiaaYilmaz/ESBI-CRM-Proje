using CRMProject.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CRMProject.Data.Seed
{
    // Uygulama ilk ayağa kalktığında rolleri ve varsayılan hesapları oluşturur
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            string[] roller = { "Admin", "Kullanici" };
            foreach (var rol in roller)
            {
                if (!await roleManager.RoleExistsAsync(rol))
                    await roleManager.CreateAsync(new IdentityRole(rol));
            }

            // Admin hesabı
            var adminEposta = "admin@esbi.com.tr";
            var adminKullanici = await userManager.FindByEmailAsync(adminEposta);

            if (adminKullanici == null)
            {
                adminKullanici = new AppUser
                {
                    UserName = adminEposta,
                    Email = adminEposta,
                    AdSoyad = "Sistem Yoneticisi",
                    Aktif = true,
                    EmailConfirmed = true
                };

                var sonuc = await userManager.CreateAsync(adminKullanici, "Admin@123!");
                if (sonuc.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminKullanici, "Admin");
                }
            }

            // Test amaçlı normal kullanıcı hesabı
            var testEposta = "kullanici@esbi.com.tr";
            var testKullanici = await userManager.FindByEmailAsync(testEposta);

            if (testKullanici == null)
            {
                testKullanici = new AppUser
                {
                    UserName = testEposta,
                    Email = testEposta,
                    AdSoyad = "Test Kullanicisi",
                    Aktif = true,
                    EmailConfirmed = true
                };

                var sonuc = await userManager.CreateAsync(testKullanici, "Kullanici@123!");
                if (sonuc.Succeeded)
                {
                    await userManager.AddToRoleAsync(testKullanici, "Kullanici");
                }
            }
        }
    }
}