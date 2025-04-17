namespace CourtDiary.Models
{
    public class Hearing
    {
        public int Id { get; set; }
        public int CaseId { get; set; }
        public DateOnly Date { get; set; }
        public string Details { get; set; }
    }
}
