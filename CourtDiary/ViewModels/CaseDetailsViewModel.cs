using CourtDiary.Models;

namespace CourtDiary.ViewModels
{
    public class CaseDetailsViewModel
    {
        public Case? Case { get; set; }
        public IEnumerable<Hearing> HearingList { get; set; } = new List<Hearing>();
    }
}
