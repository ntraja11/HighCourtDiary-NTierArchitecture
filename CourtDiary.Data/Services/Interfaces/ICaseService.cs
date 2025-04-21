using CourtDiary.Domain.Models;
using CourtDiary.Shared.ViewModels;

namespace CourtDiary.Data.Services.Interfaces
{
    public interface ICaseService
    {
        Task<CaseListViewModel> GetCaseListAsync(string lawyerId);
        Task<Case?> GetCaseAsync(int caseId);
        Task<bool> CreateCaseAsync(Case caseModel);
        Task<bool> UpdateCaseAsync(Case caseModel);
        Task<string> DeleteCaseAsync(int caseId);
        Task<CaseDetailsViewModel?> GetCaseDetailsAsync(int caseId);
    }
}
