using CourtDiary.Domain.Models;

namespace CourtDiary.Shared.ViewModels
{
    public class SuperAdminViewModel
    {
        public List<Organization> ApprovedOrganizations { get; set; } = new List<Organization>();
        public List<Organization> PendingOrganizations { get; set; } = new List<Organization>();

    }
}
