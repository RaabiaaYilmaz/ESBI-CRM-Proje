using CRMProject.Data.Entities;
using CRMProject.Services.Interfaces;
using CRMProject.Web.Models.CariViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMProject.Web.Controllers
{
    // Giriş yapmış tüm kullanıcılar erişebilir; silme işlemi ayrıca Admin'e kısıtlı (aşağıda)
    [Authorize]
    public class CariController : Controller
    {
        private readonly ICariService _cariService;

        public CariController(ICariService cariService)
        {
            _cariService = cariService;
        }

        public async Task<IActionResult> Index(string aramaMetni = "")
        {
            var cariler = await _cariService.ListeAsync(aramaMetni);
            ViewBag.AramaMetni = aramaMetni;
            return View(cariler);
        }

        [HttpGet]
        public IActionResult Ekle()
        {
            return View(new CariViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(CariViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (await _cariService.KodMevcutMuAsync(model.CariKodu))
            {
                ModelState.AddModelError(nameof(model.CariKodu), "Bu cari kodu zaten kullanılmaktadır.");
                return View(model);
            }

            var cari = new Cari
            {
                CariKodu = model.CariKodu,
                Unvan = model.Unvan,
                CariTipi = model.CariTipi,
                VergiDairesi = model.VergiDairesi,
                VergiNo = model.VergiNo,
                Telefon = model.Telefon,
                Eposta = model.Eposta,
                Adres = model.Adres,
                Il = model.Il,
                Ilce = model.Ilce
            };

            await _cariService.EkleAsync(cari);
            TempData["Basari"] = "Cari başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Duzenle(int id)
        {
            var cari = await _cariService.GetirAsync(id);
            if (cari == null) return NotFound();

            var model = new CariViewModel
            {
                Id = cari.Id,
                CariKodu = cari.CariKodu,
                Unvan = cari.Unvan,
                CariTipi = cari.CariTipi,
                VergiDairesi = cari.VergiDairesi,
                VergiNo = cari.VergiNo,
                Telefon = cari.Telefon,
                Eposta = cari.Eposta,
                Adres = cari.Adres,
                Il = cari.Il,
                Ilce = cari.Ilce
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duzenle(int id, CariViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var guncelCari = new Cari
            {
                Unvan = model.Unvan,
                CariTipi = model.CariTipi,
                VergiDairesi = model.VergiDairesi,
                VergiNo = model.VergiNo,
                Telefon = model.Telefon,
                Eposta = model.Eposta,
                Adres = model.Adres,
                Il = model.Il,
                Ilce = model.Ilce
            };

            await _cariService.GuncelleAsync(id, guncelCari);
            TempData["Basari"] = "Cari başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // Silme işlemi yalnızca Admin rolüne açık — dokümandaki güvenlik kuralı
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sil(int id)
        {
            await _cariService.PasifYapAsync(id);
            TempData["Basari"] = "Cari başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}