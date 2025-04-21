using CourtDiary.Data.Repositories.Interfaces;
using CourtDiary.Data.Services.Interfaces;
using CourtDiary.Domain.Models;
using CourtDiary.Shared.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CourtDiary.Data.Services
{
    public class CaseService : ICaseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CaseService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<CaseListViewModel> GetCaseListAsync(string lawyerId)
        {
            var lawyer = await _userManager.FindByIdAsync(lawyerId);
            return new CaseListViewModel
            {
                LawyerId = lawyerId,
                LawyerName = lawyer?.FullName ?? string.Empty,
                Cases = await _unitOfWork.Cases.GetAllAsync(c => c.LawyerId == lawyerId)
            };
        }

        public async Task<Case?> GetCaseAsync(int caseId)
        {
            return await _unitOfWork.Cases.GetAsync(c => c.Id == caseId);
        }

        public async Task<bool> CreateCaseAsync(Case caseModel)
        {
            var userEmail = caseModel.LawyerId != null ? (await _userManager.FindByIdAsync(caseModel.LawyerId))?.Email : null;
            if (string.IsNullOrEmpty(userEmail)) return false;

            await _unitOfWork.Cases.AddAsync(caseModel);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateCaseAsync(Case caseModel)
        {
            await _unitOfWork.Cases.UpdateAsync(caseModel);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<string> DeleteCaseAsync(int caseId)
        {
            var caseFromDb = await _unitOfWork.Cases.GetAsync(c => c.Id == caseId);
            if (caseFromDb == null) return "";

            await _unitOfWork.Cases.DeleteAsync(caseFromDb);
            await _unitOfWork.SaveAsync();
            return caseFromDb.LawyerId!;
        }

        public async Task<CaseDetailsViewModel?> GetCaseDetailsAsync(int caseId)
        {
            var caseFromDb = await _unitOfWork.Cases.GetAsync(c => c.Id == caseId);
            if (caseFromDb == null) return null;

            return new CaseDetailsViewModel
            {
                Case = caseFromDb,
                HearingList = await _unitOfWork.Hearings.GetAllAsync(h => h.CaseId == caseId)
            };
        }
    }
}
