using CourtDiary.Data.Context;
using CourtDiary.Data.Repositories.Interfaces;
using CourtDiary.Domain.Models;

namespace CourtDiary.Data.Repositories
{
    public class HearingRepository : Repository<Hearing>, IHearingRepository
    {
        public HearingRepository(CourtDiaryDbContext db) : base(db) { }

        public async Task UpdateAsync(Hearing hearing)
        {
            dbSet.Update(hearing);
            await Task.CompletedTask;
        }
    }
}
