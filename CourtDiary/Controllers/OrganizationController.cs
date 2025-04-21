using CourtDiary.Data.Services.Interfaces;
using CourtDiary.Shared.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CourtDiary.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IUserService userService, IOrganizationService organizationService)
        {
            _userService = userService;
            _organizationService = organizationService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userService.GetAuthenticatedUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");            

            if (!await _userService.IsOrganizationAdminAsync(user) && (await _userService.IsLawyerAsync(user) || await _userService.IsJuniorAsync(user)))
                return RedirectToAction("CaseList", "Case", new { lawyerId = user.Id });
                       
            return RedirectToAction("OrganizationDashboard", "Organization");
        }

        public async Task<IActionResult> OrganizationDashboard()
        {
            var user = await _userService.GetAuthenticatedUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            if (await _userService.IsSuperAdminAsync(user))
                return View("SuperAdmin", await _organizationService.GetSuperAdminDashboardAsync(user));

            if (await _userService.IsOrganizationAdminAsync(user))
                return View(await _organizationService.GetOrganizationAdminDashboardAsync(user));

            //Implement Access deinied view
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> ApproveOrganization(int organizationId)
        {
            if (await _organizationService.ApproveOrganizationAsync(organizationId))
                return RedirectToAction("Index");

            return BadRequest("Approval failed.");
        }

        [HttpPost]
        public async Task<IActionResult> AssignAdminAsLawyer(string orgAdminId)
        {
            if (await _organizationService.AssignAdminAsLawyerAsync(orgAdminId))
                return RedirectToAction("Index");

            return BadRequest("Failed to update role.");
        }
    }
}
