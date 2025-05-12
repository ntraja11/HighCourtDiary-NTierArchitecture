using CourtDiary.Data.Services.Interfaces;
using CourtDiary.Shared.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CourtDiary.Controllers
{
    public class LawyerController : Controller
    {
        private readonly ILawyerService _lawyerService;

        public LawyerController(ILawyerService lawyerService)
        {
            _lawyerService = lawyerService;
        }

        public async Task<IActionResult> CreateLawyer(int organizationId)
        {
            var viewModel = await _lawyerService.GetCreateLawyerViewModelAsync(organizationId);
            return viewModel != null ? View("CreateLawyer", viewModel) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreateLawyer(CreateLawyerViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            try
            {
                var success = await _lawyerService.CreateLawyerAsync(viewModel);
                if (success)
                {
                    TempData["success"] = "Lawyer created successfully.";
                    return RedirectToAction("OrganizationDashboard", "Organization");
                }                
            }
            catch(Exception ex)
            {
                TempData["error"] = ex.Message;
            }

            
            
            return View(viewModel);
        }

        public async Task<IActionResult> EditLawyer(string lawyerId)
        {
            var viewModel = await _lawyerService.GetEditLawyerViewModelAsync(lawyerId);
            return viewModel != null ? View(viewModel) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditLawyer(EditLawyerViewModel editViewModel)
        {
            if (!ModelState.IsValid) return View(editViewModel);

            var success = await _lawyerService.EditLawyerAsync(editViewModel);
            if (success)
            {
                TempData["success"] = "Lawyer updated successfully.";
                return RedirectToAction("OrganizationDashboard", "Organization");
            }
            else
                TempData["error"] = "Failed to update lawyer. Please try again.";
            return View(editViewModel);
        }

        public async Task<IActionResult> RemoveLawyer(string lawyerId)
        {
            var success = await _lawyerService.RemoveLawyerAsync(lawyerId);

            if (success)
            {
                TempData["success"] = "Lawyer deleted successfully.";
                return RedirectToAction("OrganizationDashboard", "Organization");
            }
            else
                TempData["error"] = "Failed to delete lawyer. Please try again.";

            return NotFound();
        }
    }
}
