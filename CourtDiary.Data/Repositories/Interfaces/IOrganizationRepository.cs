using CourtDiary.Domain.Models;

namespace CourtDiary.Data.Repositories.Interfaces
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        int Count();
    }
}
