using CourtDiary.Data.Services;
using CourtDiary.Data.Services.Interfaces;
using CourtDiary.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;

namespace CourtDiary.Controllers
{
    public class HearingController : Controller
    {
        private readonly IHearingService _hearingService;

        public HearingController(IHearingService hearingService)
        {
            _hearingService = hearingService;
        }

        public IActionResult CreateHearing(int caseId)
        {
            return View(new Hearing { CaseId = caseId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateHearing(Hearing hearing)
        {
            if (!ModelState.IsValid) return View(hearing);

            var success = await _hearingService.CreateHearingAsync(hearing);
            if(success) {
                TempData["success"] = "Hearing created successfully.";
                return RedirectToAction("CaseDetails", "Case", new { caseId = hearing.CaseId });
            }
            else
                TempData["error"] = "Failed to create hearing. Please try again.";

            return View(hearing);
        }

        public async Task<IActionResult> EditHearing(int hearingId)
        {
            var hearing = await _hearingService.GetHearingAsync(hearingId);
            return hearing != null ? View(hearing) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditHearing(Hearing hearing)
        {
            if (!ModelState.IsValid) return View(hearing);

            var success = await _hearingService.UpdateHearingAsync(hearing);
            if (success)
            {
                TempData["success"] = "Hearing updated successfully.";
                return RedirectToAction("CaseDetails", "Case", new { caseId = hearing.CaseId });
            }
            else
                TempData["error"] = "Failed to update hearing. Please try again.";

            return View(hearing);
        }

        public async Task<IActionResult> DeleteHearing(int hearingId)
        {
            var caseId = await _hearingService.DeleteHearingAsync(hearingId);
            if(caseId > 0)
            {
                TempData["success"] = "Hearing deleted successfully.";
                return RedirectToAction("CaseDetails", "Case", new { caseId });
            }
            else
                TempData["error"] = "Failed to delete hearing. Please try again.";

            return NotFound();
        }
    }
}
