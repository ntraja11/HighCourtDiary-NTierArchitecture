using CourtDiary.Data;
using CourtDiary.Models;
using CourtDiary.Utility;
using CourtDiary.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
                var organizationAdminViewModel = new OrganizationAdminViewModel
                {
                    Organization = await _db.Organizations
                        .FirstOrDefaultAsync(o => o.CreatedBy == user!.Email)
                };

                return View("OrganizationAdminView", organizationAdminViewModel);
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

            var orgAdmin = await _userManager.FindByEmailAsync(organization.CreatedBy);
            if(orgAdmin is not null)
            {
                orgAdmin.EmailConfirmed = true;
                await _userManager.UpdateAsync(orgAdmin);
            }
            return RedirectToAction("Index");
        }
    }
}
