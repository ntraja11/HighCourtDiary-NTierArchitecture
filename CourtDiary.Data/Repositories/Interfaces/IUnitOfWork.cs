using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtDiary.Data.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IOrganizationRepository Organizations { get; }
        ICaseRepository Cases { get; }
        IHearingRepository Hearings { get; }
        Task SaveAsync();
    }
}
