using CourtDiary.Data;
using CourtDiary.Models;
using CourtDiary.Utility;
using CourtDiary.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CourtDiary.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly CourtDiaryDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public OrganizationController(CourtDiaryDbContext db,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            if(!User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail!);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var isSuperAdmin = await _userManager.IsInRoleAsync(user, StaticDetails.RoleSuperAdmin);
            var isOrganizationAdmin = await _userManager.IsInRoleAsync(user, StaticDetails.RoleOrganizationAdmin);
            var isLawyer = await _userManager.IsInRoleAsync(user, StaticDetails.RoleLawyer);
            var isJunior = await _userManager.IsInRoleAsync(user, StaticDetails.RoleJunior);

            if (isSuperAdmin)
            {
                var organizations = await _db.Organizations.Where(o => o.CreatedBy != user!.Email).ToListAsync();


                var approvedOrganizations = organizations.Where(o => o.IsActive).ToList();
                var pendingOrganizations = organizations.Where(o => !o.IsActive).ToList();

                var superAdminViewModel = new SuperAdminViewModel
                {
                    ApprovedOrganizations = approvedOrganizations,
                    PendingOrganizations = pendingOrganizations
                };

                return View("SuperAdminView", superAdminViewModel);
            }
            else if(isOrganizationAdmin)
            {
                return await ShowOrganizationAdminView(user);
            }
            else if (isLawyer || isJunior)
            {
                return View("LawyerView");
            }

                return RedirectToAction("Index", "Home");
            
        }       


        [HttpPost]
        public async Task<IActionResult> ApproveOrganization(int organizationId)
        {
            var organization = await _db.Organizations.FindAsync(organizationId);
            if (organization != null)
            {
                organization.IsActive = true;

                organization.ActivatedDate = DateOnly.FromDateTime(DateTime.Now);
                await _db.SaveChangesAsync();
            }

            var orgAdmin = await _userManager.FindByEmailAsync(organization!.CreatedBy!);
            if(orgAdmin is not null)
            {
                orgAdmin.EmailConfirmed = true;
                await _userManager.UpdateAsync(orgAdmin);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AssignAdminAsLawyer(string orgAdminId)
        {
            var orgAdmin = await _userManager.FindByIdAsync(orgAdminId);

            if(orgAdmin is not null)
            {
                orgAdmin.IsLawyer = true;
                await _userManager.UpdateAsync(orgAdmin);
            }

            return RedirectToAction("Index");
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
                OrganizationId = organization.Id
            };
            var result = await _userManager.CreateAsync(user, viewModel.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, viewModel.SelectedRole);
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(viewModel);
        }

        private async Task<IActionResult> ShowOrganizationAdminView(ApplicationUser user)
        {
            OrganizationAdminViewModel organizationAdminViewModel = await GetOrganizationAdminViewModel(user);

            return View("OrganizationAdminView", organizationAdminViewModel);
        }

        private async Task<OrganizationAdminViewModel> GetOrganizationAdminViewModel(ApplicationUser user)
        {
            var lawyers = await _userManager.GetUsersInRoleAsync(StaticDetails.RoleLawyer);
            var organizationLawyers = lawyers.Where(l => l.OrganizationId == user.OrganizationId).ToList();

            var juniors = await _userManager.GetUsersInRoleAsync(StaticDetails.RoleJunior);
            organizationLawyers.AddRange(juniors);

            if(user.IsLawyer)
                organizationLawyers.Add(user);

            return new OrganizationAdminViewModel
            {
                Organization = await _db.Organizations
                                    .FirstOrDefaultAsync(o => o.CreatedBy == user!.Email),
                OrganizationAdmin = user,
                Lawyers = organizationLawyers,
            };
        }
    }
}
