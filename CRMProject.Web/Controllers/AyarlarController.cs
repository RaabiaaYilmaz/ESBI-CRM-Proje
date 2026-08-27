using CRMProject.Data.Entities;
using CRMProject.Web.Models.AyarlarViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CRMProject.Web.Controllers
{
    // Her giriş yapmış kullanıcı kendi ayarlarına erişebilir — yalnızca Admin'e özel değil
    [Authorize]
    public class AyarlarController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AyarlarController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new SifreDegistirViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SifreDegistirViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var kullanici = await _userManager.GetUserAsync(User);
            if (kullanici == null) return NotFound();

            var sonuc = await _userManager.ChangePasswordAsync(kullanici, model.MevcutSifre, model.YeniSifre);

            if (!sonuc.Succeeded)
            {
                foreach (var hata in sonuc.Errors)
                    ModelState.AddModelError("", hata.Description);
                return View(model);
            }

            // Şifre değiştikten sonra oturumu tazeliyoruz — kullanıcı sistemden dışarı atılmasın
            await _signInManager.RefreshSignInAsync(kullanici);

            TempData["Basari"] = "Şifreniz başarıyla değiştirildi.";
            return RedirectToAction(nameof(Index));
        }
    }
}