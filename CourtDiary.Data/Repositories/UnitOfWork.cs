using CourtDiary.Data.Context;
using CourtDiary.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtDiary.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private CourtDiaryDbContext _db;
        public IOrganizationRepository Organizations { get; private set; }
        public ICaseRepository Cases { get; private set; }
        public IHearingRepository Hearings { get; private set; }

        public UnitOfWork(CourtDiaryDbContext db)
        {
            _db = db;
            Organizations = new OrganizationRepository(_db);
            Cases = new CaseRepository(_db);
            Hearings = new HearingRepository(_db);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
