using CourtDiary.Domain.Models;
using CourtDiary.Shared.ViewModels;
using System.Security.Claims;

namespace CourtDiary.Data.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(RegistrationViewModel model);
        Task<bool> LoginAsync(LoginViewModel model);
        Task<ApplicationUser?> GetUserByEmailAsync(string email);
        Task LogoutAsync();
        Task<ApplicationUser?> GetAuthenticatedUserAsync(ClaimsPrincipal userClaims);
        Task<bool> IsSuperAdminAsync(ApplicationUser user);
        Task<bool> IsOrganizationAdminAsync(ApplicationUser user);
        Task<bool> IsLawyerAsync(ApplicationUser user);
        Task<bool> IsJuniorAsync(ApplicationUser user);

    }
}
