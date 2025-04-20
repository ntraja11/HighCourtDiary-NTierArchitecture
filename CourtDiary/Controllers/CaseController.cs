using CourtDiary.Data.Context;
using CourtDiary.Data.Models;
using CourtDiary.Data.Repositories.Interfaces;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CaseController(IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<IActionResult> CaseList(string lawyerId)
        {
            var caseListViewModel = new CaseListViewModel()
            {
                LawyerId = lawyerId,
                LawyerName = (await _userManager.FindByIdAsync(lawyerId))?.FullName ?? string.Empty,
                Cases = await _unitOfWork.Cases.GetAllAsync(c => c.LawyerId == lawyerId)
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
                await _unitOfWork.Cases.AddAsync(caseModel);
                await _unitOfWork.SaveAsync();
                return RedirectToAction("CaseList", new { lawyerId = caseModel.LawyerId });
            }
            return View(caseModel);
        }

        public async Task<IActionResult> EditCase(int caseId)
        {
            var caseFromDb = await _unitOfWork.Cases.GetAsync(c => c.Id == caseId);
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
                await _unitOfWork.Cases.UpdateAsync(caseModel);
                await _unitOfWork.SaveAsync();
                return RedirectToAction("CaseList", new { lawyerId = caseModel.LawyerId });
            }
            return View(caseModel);
        }


        //[Authorize(Roles = $"{StaticDetails.RoleSuperAdmin},{StaticDetails.RoleOrganizationAdmin}")]
        public async Task<IActionResult> DeleteCase(int caseId)
        {
            var caseFromDb = await _unitOfWork.Cases.GetAsync(c => c.Id == caseId);

            if(caseFromDb is not null)
            {
                await _unitOfWork.Cases.DeleteAsync(caseFromDb);
                await _unitOfWork.SaveAsync();               
                
            }

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail!);

            return RedirectToAction("Cases", new { lawyerId = user!.Id });
        }

        public async Task<IActionResult> CaseDetails(int caseId)
        {
            var caseFromDb = await _unitOfWork.Cases.GetAsync(c => c.Id == caseId);

            if (caseFromDb == null)
            {
                return NotFound();
            }
            var caseDetailsViewModel = new CaseDetailsViewModel
            {
                Case = caseFromDb,
                HearingList = await _unitOfWork.Hearings.GetAllAsync(h => h.CaseId == caseId)
            };
            return View(caseDetailsViewModel);
        }
    }
}
