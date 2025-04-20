using CourtDiary.Shared.ViewModels;

namespace CourtDiary.Data.Services.Interfaces
{
    public interface ILawyerService
    {
        Task<CreateLawyerViewModel?> GetCreateLawyerViewModelAsync(int organizationId);
        Task<bool> CreateLawyerAsync(CreateLawyerViewModel viewModel);
        Task<EditLawyerViewModel?> GetEditLawyerViewModelAsync(string lawyerId);
        Task<bool> EditLawyerAsync(EditLawyerViewModel editViewModel);
        Task<bool> RemoveLawyerAsync(string lawyerId);
    }
}
