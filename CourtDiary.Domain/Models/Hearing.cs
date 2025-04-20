namespace CourtDiary.Domain.Models
{
    public class Hearing
    {
        public int Id { get; set; }
        public int CaseId { get; set; }
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        public string? Notes { get; set; }
    }
}
