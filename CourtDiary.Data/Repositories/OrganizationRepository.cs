using CourtDiary.Data.Context;
using CourtDiary.Data.Models;
using CourtDiary.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtDiary.Data.Repositories
{
    public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(CourtDiaryDbContext db) : base(db){}

        public int Count()
        {
            return dbSet.Count();
        }

    }
}
