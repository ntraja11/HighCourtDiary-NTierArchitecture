using CourtDiary.Data;
using CourtDiary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourtDiary.Controllers
{
    public class HearingController : Controller
    {
        private readonly CourtDiaryDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HearingController(CourtDiaryDbContext db,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult CreateHearing(int caseId)
        {
            var hearing = new Hearing { CaseId = caseId };
            return View(hearing);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHearing(Hearing hearing)
        {
            if (ModelState.IsValid)
            {
                _db.Hearings.Add(hearing);
                await _db.SaveChangesAsync();

                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var user = await _userManager.FindByEmailAsync(userEmail!);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                return RedirectToAction("CaseDetails", "Case", new { caseId = hearing.CaseId });
            }
            return View(hearing);
        }
    }
}
