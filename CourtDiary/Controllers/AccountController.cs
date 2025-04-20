using CourtDiary.Data.Context;
using CourtDiary.Data.Models;
using CourtDiary.Data.Repositories.Interfaces;
using CourtDiary.Data.Utility;
using CourtDiary.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourtDiary.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View(new RegistrationViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var firstOrganization = _unitOfWork.Organizations.Count() == 0;

            var organization = new Organization
            {
                Name = model.OrganizationName,
                City = model.City,
                IsActive = false,
                CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                CreatedBy = model.Email
            };
            await _unitOfWork.Organizations.AddAsync(organization);
            await _unitOfWork.SaveAsync();


            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                FullName = model.FullName,
                EmailConfirmed = true,
                OrganizationId = organization.Id                
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (firstOrganization)
                {
                    await _userManager.AddToRoleAsync(user, StaticDetails.RoleSuperAdmin);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, StaticDetails.RoleOrganizationAdmin);
                }

                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Failed to create account.");
            return View(model);
        }

        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName!, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.FullName!),
                        new Claim(ClaimTypes.Email, user.Email!),
                        new Claim(ClaimTypes.NameIdentifier, user.Id)
                    };

                var claimsIdentity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);

                await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Organization");
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
