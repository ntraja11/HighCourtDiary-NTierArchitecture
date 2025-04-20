using CourtDiary.Data.Repositories.Interfaces;
using CourtDiary.Data.Services.Interfaces;
using CourtDiary.Domain.Models;
using CourtDiary.Shared.Utility;
using CourtDiary.Shared.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CourtDiary.Data.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrganizationService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<SuperAdminViewModel> GetSuperAdminDashboardAsync(ApplicationUser user)
        {
            var organizations = await _unitOfWork.Organizations.GetAllAsync(o => o.CreatedBy != user.Email);
            return new SuperAdminViewModel
            {
                ApprovedOrganizations = organizations.Where(o => o.IsActive).ToList(),
                PendingOrganizations = organizations.Where(o => !o.IsActive).ToList()
            };
        }

        public async Task<OrganizationAdminViewModel> GetOrganizationAdminDashboardAsync(ApplicationUser user)
        {
            var lawyers = await _userManager.GetUsersInRoleAsync(StaticDetails.RoleLawyer);
            var organizationLawyers = lawyers.Where(l => l.OrganizationId == user.OrganizationId).ToList();

            var juniors = await _userManager.GetUsersInRoleAsync(StaticDetails.RoleJunior);
            organizationLawyers.AddRange(juniors);

            if (user.IsLawyer)
                organizationLawyers.Add(user);

            var cases = new List<Case>();

            foreach (var lawyer in organizationLawyers)
            {
                cases.AddRange(await _unitOfWork.Cases.GetAllAsync(c => c.LawyerId == lawyer.Id));
            }

            return new OrganizationAdminViewModel
            {
                Organization = await _unitOfWork.Organizations.GetAsync(o => o.CreatedBy == user.Email),
                OrganizationAdmin = user,
                Lawyers = organizationLawyers,
                Cases = cases
            };
        }

        public async Task<bool> ApproveOrganizationAsync(int organizationId)
        {
            var organization = await _unitOfWork.Organizations.GetAsync(o => o.Id == organizationId);
            if (organization == null) return false;

            organization.IsActive = true;
            organization.ActivatedDate = DateOnly.FromDateTime(DateTime.Now);
            await _unitOfWork.SaveAsync();

            var orgAdmin = await _userManager.FindByEmailAsync(organization.CreatedBy!);
            if (orgAdmin != null)
            {
                orgAdmin.EmailConfirmed = true;
                await _userManager.UpdateAsync(orgAdmin);
            }

            return true;
        }

        public async Task<bool> AssignAdminAsLawyerAsync(string orgAdminId)
        {
            var orgAdmin = await _userManager.FindByIdAsync(orgAdminId);
            if (orgAdmin == null) return false;

            orgAdmin.IsLawyer = true;
            await _userManager.UpdateAsync(orgAdmin);

            return true;
        }
    }
}
