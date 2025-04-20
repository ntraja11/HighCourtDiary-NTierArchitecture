using CourtDiary.Data.Repositories.Interfaces;
using CourtDiary.Data.Services.Interfaces;
using CourtDiary.Domain.Models;
using CourtDiary.Shared.Utility;
using CourtDiary.Shared.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CourtDiary.Data.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> RegisterAsync(RegistrationViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Email))
            {
                var firstOrganization = _unitOfWork.Organizations.Count() == 0;

                var organization = new Organization
                {
                    Name = model.OrganizationName,
                    City = model.City,
                    IsActive = false,
                    CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                    CreatedBy = model.Email
                };
                await _unitOfWork.Organizations.AddAsync(organization);
                await _unitOfWork.SaveAsync();

                var user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    FullName = model.FullName,
                    EmailConfirmed = true,
                    OrganizationId = organization.Id
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var role = firstOrganization ? StaticDetails.RoleSuperAdmin : StaticDetails.RoleOrganizationAdmin;
                    await _userManager.AddToRoleAsync(user, role);
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> LoginAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null) return false;

            var result = await _signInManager.PasswordSignInAsync(user.UserName!, model.Password, model.RememberMe, lockoutOnFailure: false);
            return result.Succeeded;
        }

        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<ApplicationUser?> GetAuthenticatedUserAsync(ClaimsPrincipal userClaims)
        {
            if (!userClaims.Identity!.IsAuthenticated) return null;

            var userEmail = userClaims.FindFirstValue(ClaimTypes.Email);
            return await _userManager.FindByEmailAsync(userEmail!);
        }

        public async Task<bool> IsSuperAdminAsync(ApplicationUser user)
        {
            return await _userManager.IsInRoleAsync(user, StaticDetails.RoleSuperAdmin);
        }

        public async Task<bool> IsOrganizationAdminAsync(ApplicationUser user)
        {
            return await _userManager.IsInRoleAsync(user, StaticDetails.RoleOrganizationAdmin);
        }

        public async Task<bool> IsLawyerAsync(ApplicationUser user)
        {
            return await _userManager.IsInRoleAsync(user, StaticDetails.RoleLawyer) || user.IsLawyer;
        }

        public async Task<bool> IsJuniorAsync(ApplicationUser user)
        {
            return await _userManager.IsInRoleAsync(user, StaticDetails.RoleJunior);
        }
    }
}

