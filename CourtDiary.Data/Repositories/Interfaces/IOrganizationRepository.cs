using CourtDiary.Data.Models;

namespace CourtDiary.Data.Repositories.Interfaces
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        int Count();  
    }
}
