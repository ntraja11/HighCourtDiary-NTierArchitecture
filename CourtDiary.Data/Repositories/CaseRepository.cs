using CourtDiary.Data.Context;
using CourtDiary.Data.Models;
using CourtDiary.Data.Repositories.Interfaces;

namespace CourtDiary.Data.Repositories
{
    public class CaseRepository : Repository<Case>, ICaseRepository
    {
        public CaseRepository(CourtDiaryDbContext db) : base(db){}

        public async Task UpdateAsync(Case caseEntity)
        {
            dbSet.Update(caseEntity);
            await Task.CompletedTask;
        }        
    }
}
