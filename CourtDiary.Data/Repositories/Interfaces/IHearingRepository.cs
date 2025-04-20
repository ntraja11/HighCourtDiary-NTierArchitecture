using CourtDiary.Domain.Models;

namespace CourtDiary.Data.Repositories.Interfaces
{
    public interface IHearingRepository : IRepository<Hearing>
    {
        Task UpdateAsync(Hearing hearing);
    }
}
