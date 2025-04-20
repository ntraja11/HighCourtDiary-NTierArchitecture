using CourtDiary.Data.Context;
using CourtDiary.Data.Repositories.Interfaces;
using CourtDiary.Domain.Models;

namespace CourtDiary.Data.Repositories
{
    public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(CourtDiaryDbContext db) : base(db) { }

        public int Count()
        {
            return dbSet.Count();
        }

    }
}
