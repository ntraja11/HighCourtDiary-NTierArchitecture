using CourtDiary.Data.Models;

namespace CourtDiary.ViewModels
{
    public class SuperAdminViewModel
    {
        public List<Organization> ApprovedOrganizations { get; set; } = new List<Organization>();
        public List<Organization> PendingOrganizations { get; set; } = new List<Organization>();

    }
}
