using CourtDiary.Domain.Models;
using CourtDiary.Shared.ViewModels;

namespace CourtDiary.Data.Services.Interfaces
{
    public interface IOrganizationService
    {
        Task<SuperAdminViewModel> GetSuperAdminDashboardAsync(ApplicationUser user);
        Task<OrganizationAdminViewModel> GetOrganizationAdminDashboardAsync(ApplicationUser user);
        Task<bool> ApproveOrganizationAsync(int organizationId);
        Task<bool> AssignAdminAsLawyerAsync(string orgAdminId);
    }
}
