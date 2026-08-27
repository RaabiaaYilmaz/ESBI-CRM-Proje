using CRMProject.Services.Interfaces;
using CRMProject.Web.Models.DashboardViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMProject.Web.Controllers
{
    // Giriş yapmış tüm kullanıcılar erişebilir (Admin ve Kullanici rolleri)
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ICariService _cariService;
        private readonly IMalzemeService _malzemeService;

        public DashboardController(ICariService cariService, IMalzemeService malzemeService)
        {
            _cariService = cariService;
            _malzemeService = malzemeService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel
            {
                ToplamCari = await _cariService.ToplamSayiAsync(),
                ToplamMalzeme = await _malzemeService.ToplamSayiAsync(),
                KritikStoklar = await _malzemeService.KritikStoklarAsync(),
                SonCariler = await _cariService.SonEklenenlerAsync(5)
            };

            return View(model);
        }
    }
}