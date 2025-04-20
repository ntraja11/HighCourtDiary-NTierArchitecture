using CourtDiary.Domain.Models;

namespace CourtDiary.Data.Repositories.Interfaces
{
    public interface ICaseRepository : IRepository<Case>
    {
        Task UpdateAsync(Case caseEntity);
    }
}
