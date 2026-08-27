using CRMProject.Data.Entities;
using CRMProject.Services.Interfaces;
using CRMProject.Web.Models.MalzemeViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMProject.Web.Controllers
{
    [Authorize]
    public class MalzemeController : Controller
    {
        private readonly IMalzemeService _malzemeService;

        public MalzemeController(IMalzemeService malzemeService)
        {
            _malzemeService = malzemeService;
        }

        public async Task<IActionResult> Index(string aramaMetni = "", string kategori = "")
        {
            var malzemeler = await _malzemeService.ListeAsync(aramaMetni, kategori);
            ViewBag.Kategoriler = await _malzemeService.KategoriListesiAsync();
            ViewBag.AramaMetni = aramaMetni;
            ViewBag.SeciliKategori = kategori;
            return View(malzemeler);
        }

        [HttpGet]
        public async Task<IActionResult> Ekle()
        {
            ViewBag.Kategoriler = await _malzemeService.KategoriListesiAsync();
            return View(new MalzemeViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(MalzemeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Kategoriler = await _malzemeService.KategoriListesiAsync();
                return View(model);
            }

            if (await _malzemeService.KodMevcutMuAsync(model.MalzemeKodu))
            {
                ModelState.AddModelError(nameof(model.MalzemeKodu), "Bu malzeme kodu zaten mevcuttur.");
                ViewBag.Kategoriler = await _malzemeService.KategoriListesiAsync();
                return View(model);
            }

            var malzeme = new Malzeme
            {
                MalzemeKodu = model.MalzemeKodu,
                MalzemeAdi = model.MalzemeAdi,
                Kategori = model.Kategori,
                Birim = model.Birim,
                SatisFiyati = model.SatisFiyati,
                AlisFiyati = model.AlisFiyati,
                StokMiktari = model.StokMiktari,
                KritikStok = model.KritikStok,
                Aciklama = model.Aciklama
            };

            await _malzemeService.EkleAsync(malzeme);
            TempData["Basari"] = "Malzeme başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Duzenle(int id)
        {
            var malzeme = await _malzemeService.GetirAsync(id);
            if (malzeme == null) return NotFound();

            ViewBag.Kategoriler = await _malzemeService.KategoriListesiAsync();

            var model = new MalzemeViewModel
            {
                Id = malzeme.Id,
                MalzemeKodu = malzeme.MalzemeKodu,
                MalzemeAdi = malzeme.MalzemeAdi,
                Kategori = malzeme.Kategori,
                Birim = malzeme.Birim,
                SatisFiyati = malzeme.SatisFiyati,
                AlisFiyati = malzeme.AlisFiyati,
                StokMiktari = malzeme.StokMiktari,
                KritikStok = malzeme.KritikStok,
                Aciklama = malzeme.Aciklama
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duzenle(int id, MalzemeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Kategoriler = await _malzemeService.KategoriListesiAsync();
                return View(model);
            }

            var guncelMalzeme = new Malzeme
            {
                MalzemeAdi = model.MalzemeAdi,
                Kategori = model.Kategori,
                Birim = model.Birim,
                SatisFiyati = model.SatisFiyati,
                AlisFiyati = model.AlisFiyati,
                StokMiktari = model.StokMiktari,
                KritikStok = model.KritikStok,
                Aciklama = model.Aciklama
            };

            await _malzemeService.GuncelleAsync(id, guncelMalzeme);
            TempData["Basari"] = "Malzeme başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // Silme işlemi yalnızca Admin rolüne açık
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sil(int id)
        {
            await _malzemeService.PasifYapAsync(id);
            TempData["Basari"] = "Malzeme başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}