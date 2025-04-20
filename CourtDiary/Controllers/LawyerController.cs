using CourtDiary.Data.Context;
using CourtDiary.Data.Models;
using CourtDiary.Data.Utility;
using CourtDiary.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace CourtDiary.Controllers
{
    public class LawyerController : Controller
    {
        private readonly CourtDiaryDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LawyerController(CourtDiaryDbContext db,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> CreateLawyer(int organizationId)
        {
            var organization = await _db.Organizations.FindAsync(organizationId);
            if (organization == null)
            {
                return NotFound();
            }
            var createLawyerViewModel = new CreateLawyerViewModel
            {
                Lawyer = new ApplicationUser
                {
                    OrganizationId = organization.Id
                }
            };
            return View("CreateLawyer", createLawyerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLawyer(CreateLawyerViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var organization = await _db.Organizations.FindAsync(viewModel.Lawyer!.OrganizationId);
            if (organization == null)
            {
                return NotFound();
            }
            var user = new ApplicationUser
            {
                Email = viewModel.Lawyer.Email,
                UserName = viewModel.Lawyer.Email,
                FullName = viewModel.Lawyer.FullName,
                EmailConfirmed = true,
                OrganizationId = organization.Id,
                IsJunior = (viewModel!.SelectedRole == StaticDetails.RoleJunior)
                
            };
            var result = await _userManager.CreateAsync(user, viewModel!.Password!);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, StaticDetails.RoleLawyer);

                return RedirectToAction("Index", "Organization");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(viewModel);
        }

        public async Task<IActionResult> EditLawyer(string lawyerId)
        {
            var lawyer = await _userManager.FindByIdAsync(lawyerId);
            if (lawyer == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(lawyer);

            return View(GetEditViewModel(lawyer, roles));
        }

        private static EditLawyerViewModel GetEditViewModel(ApplicationUser lawyer, IList<string> roles)
        {
            return new EditLawyerViewModel
            {
                Lawyer = lawyer,
                SelectedRole = roles.FirstOrDefault(),
            };
        }

        [HttpPost]
        public async Task<IActionResult> EditLawyer(EditLawyerViewModel editViewmodel)
        {
            if (ModelState.IsValid)
            {
                var lawyerFromDb = await _userManager.FindByIdAsync(editViewmodel.Lawyer!.Id);

                if(lawyerFromDb == null) { return NotFound(); }

                lawyerFromDb.FullName = editViewmodel.Lawyer.FullName;
                lawyerFromDb.Email = editViewmodel.Lawyer.Email;
                lawyerFromDb.NormalizedEmail = editViewmodel.Lawyer.Email!.ToUpper();
                lawyerFromDb.UserName = editViewmodel.Lawyer.Email;
                lawyerFromDb.NormalizedUserName = editViewmodel.Lawyer.Email.ToUpper();
                lawyerFromDb.PhoneNumber = editViewmodel.Lawyer.PhoneNumber;
                lawyerFromDb.IsJunior = (editViewmodel!.SelectedRole == StaticDetails.RoleJunior);
                
                var result = await _userManager.UpdateAsync(lawyerFromDb);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Organization");
                }                               
            }

            var roles = await _userManager.GetRolesAsync(editViewmodel.Lawyer!);

            return View(GetEditViewModel(editViewmodel.Lawyer!, roles));
        }

        public async Task<IActionResult> RemoveLawyer(string lawyerId)
        {
            var lawyer = await _userManager.FindByIdAsync(lawyerId);

            if(lawyer == null) { return NotFound(); }

            var result = await _userManager.DeleteAsync(lawyer);
            if (result.Succeeded) 
            {
                return RedirectToAction("Index", "Organization");
            }

            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return RedirectToAction("Index", "Organization");
        }
    }
}
