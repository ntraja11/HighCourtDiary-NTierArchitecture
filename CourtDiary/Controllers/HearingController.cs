using CourtDiary.Data.Services.Interfaces;
using CourtDiary.Domain.Models;
using Microsoft.AspNetCore.Mvc;

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
            return success ? RedirectToAction("CaseDetails", "Case", new { caseId = hearing.CaseId }) : View(hearing);
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
            return success ? RedirectToAction("CaseDetails", "Case", new { caseId = hearing.CaseId }) : View(hearing);
        }

        public async Task<IActionResult> DeleteHearing(int hearingId)
        {
            var success = await _hearingService.DeleteHearingAsync(hearingId);
            return success ? RedirectToAction("CaseDetails", "Case") : NotFound();
        }
    }
}
