using CourtDiary.Data.Repositories.Interfaces;
using CourtDiary.Data.Services.Interfaces;
using CourtDiary.Domain.Models;

namespace CourtDiary.Data.Services
{
    public class HearingService : IHearingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HearingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Hearing?> GetHearingAsync(int hearingId)
        {
            return await _unitOfWork.Hearings.GetAsync(h => h.Id == hearingId);
        }

        public async Task<bool> CreateHearingAsync(Hearing hearing)
        {
            if (hearing == null) return false;

            await _unitOfWork.Hearings.AddAsync(hearing);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateHearingAsync(Hearing hearing)
        {
            if (hearing == null) return false;

            await _unitOfWork.Hearings.UpdateAsync(hearing);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<int> DeleteHearingAsync(int hearingId)
        {
            var hearingToDelete = await _unitOfWork.Hearings.GetAsync(h => h.Id == hearingId);
            if (hearingToDelete == null) return 0;

            await _unitOfWork.Hearings.DeleteAsync(hearingToDelete);
            await _unitOfWork.SaveAsync();
            return hearingToDelete.CaseId;
        }
    }
}
