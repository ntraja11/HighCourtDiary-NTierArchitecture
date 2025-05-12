using CourtDiary.Data.Repositories.Interfaces;
using CourtDiary.Data.Services.Interfaces;
using CourtDiary.Domain.Models;
using CourtDiary.Shared.Utility;
using CourtDiary.Shared.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CourtDiary.Data.Services
{
    public class LawyerService : ILawyerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public LawyerService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<CreateLawyerViewModel?> GetCreateLawyerViewModelAsync(int organizationId)
        {
            var organization = await _unitOfWork.Organizations.GetAsync(o => o.Id == organizationId);
            if (organization == null) return null;

            return new CreateLawyerViewModel
            {
                Lawyer = new ApplicationUser { OrganizationId = organization.Id }
            };
        }

        public async Task<bool> CreateLawyerAsync(CreateLawyerViewModel viewModel)
        {
            if (!new EmailAddressAttribute().IsValid(viewModel.Lawyer!.Email)) return false;

            var organization = await _unitOfWork.Organizations.GetAsync(o => o.Id == viewModel.Lawyer.OrganizationId);
            if (organization == null) return false;

            var user = new ApplicationUser
            {
                Email = viewModel.Lawyer.Email,
                UserName = viewModel.Lawyer.Email,
                FullName = viewModel.Lawyer.FullName,
                EmailConfirmed = true,
                OrganizationId = organization.Id,
                IsJunior = viewModel.SelectedRole == StaticDetails.RoleJunior
            };

            try
            {
                var result = await _userManager.CreateAsync(user, viewModel.Password!);

                if (!result.Succeeded)
                {
                    var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
                    throw new Exception($"User creation failed: {errorMessages}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            await _userManager.AddToRoleAsync(user, StaticDetails.RoleLawyer);
            return true;
        }

        public async Task<EditLawyerViewModel?> GetEditLawyerViewModelAsync(string lawyerId)
        {
            var lawyer = await _userManager.FindByIdAsync(lawyerId);
            if (lawyer == null) return null;

            var roles = await _userManager.GetRolesAsync(lawyer);
            return new EditLawyerViewModel { Lawyer = lawyer, SelectedRole = roles.FirstOrDefault() };
        }

        public async Task<bool> EditLawyerAsync(EditLawyerViewModel editViewModel)
        {
            var lawyerFromDb = await _userManager.FindByIdAsync(editViewModel.Lawyer!.Id);
            if (lawyerFromDb == null) return false;

            lawyerFromDb.FullName = editViewModel.Lawyer.FullName;
            lawyerFromDb.Email = editViewModel.Lawyer.Email;
            lawyerFromDb.NormalizedEmail = editViewModel.Lawyer.Email!.ToUpper();
            lawyerFromDb.UserName = editViewModel.Lawyer.Email;
            lawyerFromDb.NormalizedUserName = editViewModel.Lawyer.Email.ToUpper();
            lawyerFromDb.PhoneNumber = editViewModel.Lawyer.PhoneNumber;
            lawyerFromDb.IsJunior = editViewModel.SelectedRole == StaticDetails.RoleJunior;

            var result = await _userManager.UpdateAsync(lawyerFromDb);
            return result.Succeeded;
        }

        public async Task<bool> RemoveLawyerAsync(string lawyerId)
        {
            var lawyer = await _userManager.FindByIdAsync(lawyerId);
            if (lawyer == null) return false;

            var result = await _userManager.DeleteAsync(lawyer);
            return result.Succeeded;
        }
    }
}
