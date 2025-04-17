using CourtDiary.Models;

namespace CourtDiary.ViewModels
{
    public class OrganizationViewModel
    {
        public List<Organization> ApprovedOrganizations { get; set; } = new List<Organization>();
        public List<Organization> PendingOrganizations { get; set; } = new List<Organization>();
    }
}
