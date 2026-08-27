using CRMProject.Data.Entities;
using CRMProject.Web.Models.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CRMProject.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new GirisViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(GirisViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var kullanici = await _userManager.FindByEmailAsync(model.Eposta);

            if (kullanici == null || !kullanici.Aktif)
            {
                ModelState.AddModelError("", "E-posta veya şifre hatalı.");
                return View(model);
            }

            var sonuc = await _signInManager.PasswordSignInAsync(
                kullanici.UserName!, model.Sifre, model.BeniHatirla, lockoutOnFailure: true);

            if (sonuc.Succeeded)
                return RedirectToAction("Index", "Dashboard");

            if (sonuc.IsLockedOut)
                ModelState.AddModelError("", "Hesabınız çok fazla hatalı denemeden dolayı geçici olarak kilitlendi.");
            else
                ModelState.AddModelError("", "E-posta veya şifre hatalı.");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}