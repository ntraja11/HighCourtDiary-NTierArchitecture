using CourtDiary.Data.Models;

namespace CourtDiary.Data.Repositories.Interfaces
{
    public interface ICaseRepository : IRepository<Case>
    {
        Task UpdateAsync(Case caseEntity);
    }
}
