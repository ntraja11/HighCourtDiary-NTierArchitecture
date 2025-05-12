using CourtDiary.Domain.Models;

namespace CourtDiary.Data.Services.Interfaces
{
    public interface IHearingService
    {
        Task<Hearing?> GetHearingAsync(int hearingId);
        Task<bool> CreateHearingAsync(Hearing hearing);
        Task<bool> UpdateHearingAsync(Hearing hearing);
        Task<int> DeleteHearingAsync(int hearingId);
    }
}
