using CourtDiary.Data.Context;
using CourtDiary.Data.Models;
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
        public async Task<IActionResult> CaseList(string lawyerId)
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
                return RedirectToAction("CaseList", new { lawyerId = caseModel.LawyerId });
            }
            return View(caseModel);
        }

        public async Task<IActionResult> EditCase(int caseId)
        {
            var caseFromDb = await _db.Cases.FindAsync(caseId);
            if (caseFromDb == null)
            {
                return NotFound();
            }
            return View(caseFromDb);
        }

        [HttpPost]
        public async Task<IActionResult> EditCase(Case caseModel)
        {
            if (ModelState.IsValid)
            {
                _db.Cases.Update(caseModel);
                await _db.SaveChangesAsync();
                return RedirectToAction("CaseList", new { lawyerId = caseModel.LawyerId });
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

        public async Task<IActionResult> CaseDetails(int caseId)
        {
            var caseFromDb = await _db.Cases.SingleOrDefaultAsync(c => c.Id == caseId);

            if (caseFromDb == null)
            {
                return NotFound();
            }
            var caseDetailsViewModel = new CaseDetailsViewModel
            {
                Case = caseFromDb,
                HearingList = await _db.Hearings.Where(h => h.CaseId == caseId).ToListAsync()
            };
            return View(caseDetailsViewModel);
        }
    }
}
