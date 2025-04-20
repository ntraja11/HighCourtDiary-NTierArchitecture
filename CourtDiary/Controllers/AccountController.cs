using CourtDiary.Data.Services.Interfaces;
using CourtDiary.Shared.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CourtDiary.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Register() => View(new RegistrationViewModel());

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var success = await _userService.RegisterAsync(model);
            if (success) return RedirectToAction("Login");

            ModelState.AddModelError("", "Failed to create account.");
            return View(model);
        }

        public IActionResult Login() => View(new LoginViewModel());

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var success = await _userService.LoginAsync(model);
            return success ? RedirectToAction("Index", "Organization") : View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
