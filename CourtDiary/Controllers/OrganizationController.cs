using CourtDiary.Data;
using CourtDiary.Models;
using CourtDiary.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var organizations = await _db.Organizations.ToListAsync();


            var approvedOrganizations = organizations.Where(o => o.IsActive).ToList();
            var pendingOrganizations = organizations.Where(o => !o.IsActive).ToList();

            var viewModel = new OrganizationViewModel
            {
                ApprovedOrganizations = approvedOrganizations,
                PendingOrganizations = pendingOrganizations
            };

            return View(viewModel);
        }
    }
}
