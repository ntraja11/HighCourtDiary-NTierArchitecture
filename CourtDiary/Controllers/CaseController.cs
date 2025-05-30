﻿using CourtDiary.Data.Services.Interfaces;
using CourtDiary.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace CourtDiary.Controllers
{
    public class CaseController : Controller
    {
        private readonly ICaseService _caseService;

        public CaseController(ICaseService caseService)
        {
            _caseService = caseService;
        }

        public async Task<IActionResult> CaseList(string lawyerId)
        {
            return View(await _caseService.GetCaseListAsync(lawyerId));
        }

        public IActionResult CreateCase(string lawyerId)
        {
            return View(new Case { LawyerId = lawyerId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCase(Case caseModel)
        {
            if (!ModelState.IsValid) return View(caseModel);

            var success = await _caseService.CreateCaseAsync(caseModel);
            if (success)
            {
                TempData["success"] = "Case created successfully.";
                return RedirectToAction("CaseList", new { lawyerId = caseModel.LawyerId });
            }
            else
                TempData["error"] = "Failed to create case. Please try again.";

            return View(caseModel);
        }

        public async Task<IActionResult> EditCase(int caseId)
        {
            var caseFromDb = await _caseService.GetCaseAsync(caseId);
            return caseFromDb != null ? View(caseFromDb) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditCase(Case caseModel)
        {
            if (!ModelState.IsValid) return View(caseModel);

            var success = await _caseService.UpdateCaseAsync(caseModel);
            if (success)
            {
                TempData["success"] = "Case updated successfully.";
                return RedirectToAction("CaseList", new { lawyerId = caseModel.LawyerId });
            }
            else
                TempData["error"] = "Failed to update case. Please try again.";

            return View(caseModel);
        }

        public async Task<IActionResult> DeleteCase(int caseId)
        {
            var lawyerId = await _caseService.DeleteCaseAsync(caseId);

            if(!lawyerId.IsNullOrEmpty())
            {
                TempData["success"] = "Case deleted successfully.";
                return RedirectToAction("CaseList", new { lawyerId });
            }
            else
                TempData["error"] = "Failed to delete case. Please try again.";

            return NotFound();
        }
        
        public async Task<IActionResult> CaseDetails(int caseId)
        {
            var caseDetailsViewModel = await _caseService.GetCaseDetailsAsync(caseId);
            return caseDetailsViewModel != null ? View(caseDetailsViewModel) : NotFound();
        }
    }
}
