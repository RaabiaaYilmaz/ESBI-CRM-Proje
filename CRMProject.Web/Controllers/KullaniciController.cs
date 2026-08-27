using CRMProject.Services.Interfaces;
using CRMProject.Web.Models.KullaniciViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CRMProject.Web.Controllers
{
    // Bu controller'ın tamamı yalnızca Admin rolüne açık
    [Authorize(Roles = "Admin")]
    public class KullaniciController : Controller
    {
        private readonly IKullaniciService _kullaniciService;

        public KullaniciController(IKullaniciService kullaniciService)
        {
            _kullaniciService = kullaniciService;
        }

        public async Task<IActionResult> Index()
        {
            var kullanicilar = await _kullaniciService.ListeAsync();
            // Görünümde "bu hesap sizsiniz" ayrımını yapabilmek için mevcut kullanıcının kimliğini gönderiyoruz
            ViewBag.MevcutKullaniciId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(kullanicilar);
        }

        [HttpGet]
        public IActionResult Ekle()
        {
            return View(new KullaniciEkleViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(KullaniciEkleViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var (basarili, hata) = await _kullaniciService.EkleAsync(
                model.Eposta, model.AdSoyad, model.Sifre, model.Rol);

            if (!basarili)
            {
                ModelState.AddModelError("", hata);
                return View(model);
            }

            TempData["Basari"] = "Kullanıcı başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RolDegistir(RolDegistirViewModel model)
        {
            var mevcutKullaniciId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Bir admin kendi rolünü değiştiremez — aksi halde sistemde hiç admin kalmayabilir
            if (model.KullaniciId == mevcutKullaniciId)
            {
                TempData["Hata"] = "Kendi hesabınızın rolünü buradan değiştiremezsiniz.";
                return RedirectToAction(nameof(Index));
            }

            await _kullaniciService.RolDegistirAsync(model.KullaniciId, model.YeniRol);
            TempData["Basari"] = "Kullanıcının rolü güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AktifPasifYap(string kullaniciId, bool aktif)
        {
            var mevcutKullaniciId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Bir admin kendi hesabını pasifleştiremez — kilitlenip dışarıda kalmasın
            if (kullaniciId == mevcutKullaniciId)
            {
                TempData["Hata"] = "Kendi hesabınızı pasifleştiremezsiniz.";
                return RedirectToAction(nameof(Index));
            }

            await _kullaniciService.AktifPasifYapAsync(kullaniciId, aktif);
            TempData["Basari"] = aktif ? "Kullanıcı aktifleştirildi." : "Kullanıcı pasifleştirildi.";
            return RedirectToAction(nameof(Index));
        }
    }
}