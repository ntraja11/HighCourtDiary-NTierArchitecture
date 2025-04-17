using CourtDiary.Data;
using CourtDiary.Models;
using CourtDiary.Utility;
using CourtDiary.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CourtDiary.Controllers
{
    public class CaseController : Controller
    {
        private readonly CourtDiaryDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public CaseController(CourtDiaryDbContext db,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Cases(string lawyerId)
        {
            var caseListViewModel = new CaseListViewModel()
            {
                LawyerId = lawyerId,
                LawyerName = (await _userManager.FindByIdAsync(lawyerId))?.FullName ?? string.Empty,
                Cases = await _db.Cases.Where(c => c.LawyerId == lawyerId).ToListAsync()
            };

            return View(caseListViewModel);
        }

        public IActionResult CreateCase(string lawyerId)
        {
            return View(new Case { LawyerId = lawyerId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCase(Case caseModel)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var user = await _userManager.FindByEmailAsync(userEmail!);

                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                //caseModel.LawyerId = user.Id;
                _db.Cases.Add(caseModel);
                await _db.SaveChangesAsync();
                return RedirectToAction("Cases", new { lawyerId = caseModel.LawyerId });
            }
            return View(caseModel);
        }

        //[Authorize(Roles = $"{StaticDetails.RoleSuperAdmin},{StaticDetails.RoleOrganizationAdmin}")]
        public async Task<IActionResult> DeleteCase(int caseId)
        {
            var caseFromDb = await _db.Cases.AsNoTracking().SingleOrDefaultAsync(c => c.Id == caseId);

            if(caseFromDb is not null)
            {
                _db.Cases.Remove(caseFromDb);
                await _db.SaveChangesAsync();               
                
            }

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail!);

            return RedirectToAction("Cases", new { lawyerId = user!.Id });
        }
    }
}
