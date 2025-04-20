using CourtDiary.Data.Models;

namespace CourtDiary.Data.Repositories.Interfaces
{
    public interface IHearingRepository : IRepository<Hearing>
    {
        Task UpdateAsync(Hearing hearing);
    }
}
