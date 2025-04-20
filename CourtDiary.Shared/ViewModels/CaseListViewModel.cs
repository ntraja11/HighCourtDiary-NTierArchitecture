using CourtDiary.Domain.Models;

namespace CourtDiary.Shared.ViewModels
{
    public class CaseListViewModel
    {
        public IEnumerable<Case> Cases { get; set; } = new List<Case>();
        public string? LawyerId { get; set; }
        public string LawyerName { get; set; } = string.Empty;
    }
}
