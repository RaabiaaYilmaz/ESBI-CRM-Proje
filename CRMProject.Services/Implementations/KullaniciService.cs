using CRMProject.Data.Entities;
using CRMProject.Services.Interfaces;
using CRMProject.Services.Models;
using Microsoft.AspNetCore.Identity;

namespace CRMProject.Services.Implementations
{
    // Kullanıcı Yönetimi ekranının ihtiyaç duyduğu işlemler — Identity'nin UserManager/RoleManager'ını sarmalıyor
    public class KullaniciService : IKullaniciService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public KullaniciService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<KullaniciListeItem>> ListeAsync()
        {
            var kullanicilar = _userManager.Users.ToList();
            var sonuc = new List<KullaniciListeItem>();

            foreach (var kullanici in kullanicilar)
            {
                var roller = await _userManager.GetRolesAsync(kullanici);
                sonuc.Add(new KullaniciListeItem
                {
                    Id = kullanici.Id,
                    AdSoyad = kullanici.AdSoyad,
                    Eposta = kullanici.Email ?? string.Empty,
                    Rol = roller.FirstOrDefault() ?? "Rol Yok",
                    Aktif = kullanici.Aktif
                });
            }

            return sonuc.OrderBy(k => k.AdSoyad).ToList();
        }

        public async Task<(bool basarili, string hata)> EkleAsync(string eposta, string adSoyad, string sifre, string rol)
        {
            var mevcut = await _userManager.FindByEmailAsync(eposta);
            if (mevcut != null)
                return (false, "Bu e-posta adresi zaten kayıtlı.");

            if (!await _roleManager.RoleExistsAsync(rol))
                return (false, "Geçersiz rol seçimi.");

            var yeniKullanici = new AppUser
            {
                UserName = eposta,
                Email = eposta,
                AdSoyad = adSoyad,
                Aktif = true,
                EmailConfirmed = true
            };

            var sonuc = await _userManager.CreateAsync(yeniKullanici, sifre);
            if (!sonuc.Succeeded)
            {
                var hatalar = string.Join(" ", sonuc.Errors.Select(e => e.Description));
                return (false, hatalar);
            }

            await _userManager.AddToRoleAsync(yeniKullanici, rol);
            return (true, string.Empty);
        }

        public async Task RolDegistirAsync(string kullaniciId, string yeniRol)
        {
            var kullanici = await _userManager.FindByIdAsync(kullaniciId);
            if (kullanici == null) return;

            var mevcutRoller = await _userManager.GetRolesAsync(kullanici);
            await _userManager.RemoveFromRolesAsync(kullanici, mevcutRoller);
            await _userManager.AddToRoleAsync(kullanici, yeniRol);
        }

        public async Task AktifPasifYapAsync(string kullaniciId, bool aktif)
        {
            var kullanici = await _userManager.FindByIdAsync(kullaniciId);
            if (kullanici == null) return;

            kullanici.Aktif = aktif;

            // Pasif yapılan kullanıcının mevcut oturumu varsa, kilitleyerek yeni giriş yapmasını da engelliyoruz
            if (!aktif)
                kullanici.LockoutEnd = DateTimeOffset.MaxValue;
            else
                kullanici.LockoutEnd = null;

            await _userManager.UpdateAsync(kullanici);
        }
    }
}