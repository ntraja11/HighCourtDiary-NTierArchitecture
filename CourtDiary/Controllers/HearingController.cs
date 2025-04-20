using CourtDiary.Data.Context;
using CourtDiary.Data.Models;
using CourtDiary.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourtDiary.Controllers
{
    public class HearingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public HearingController(IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
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
                await _unitOfWork.Hearings.AddAsync(hearing);
                await _unitOfWork.SaveAsync();

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

        public async Task<IActionResult> EditHearing(int hearingId)
        {
            var hearing = await _unitOfWork.Hearings.GetAsync(o => o.Id == hearingId);
            if (hearing == null)
            {
                return NotFound();
            }
            return View(hearing);
        }

        [HttpPost]
        public async Task<IActionResult> EditHearing(Hearing hearing)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Hearings.UpdateAsync(hearing);
                await _unitOfWork.SaveAsync();
                return RedirectToAction("CaseDetails", "Case", new { caseId = hearing.CaseId });
            }
            return View(hearing);
        }                

        
        public async Task<IActionResult> DeleteHearing(int hearingId)
        {
            var hearingToDelete = await _unitOfWork.Hearings.GetAsync(h => h.Id == hearingId);
            if (hearingToDelete == null)
            {
                return NotFound();
            }
            await _unitOfWork.Hearings.DeleteAsync(hearingToDelete);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("CaseDetails", "Case", new { caseId = hearingToDelete.CaseId });
        }
    }
}
