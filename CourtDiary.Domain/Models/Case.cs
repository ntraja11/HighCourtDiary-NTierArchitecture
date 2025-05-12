using System.ComponentModel.DataAnnotations;

namespace CourtDiary.Domain.Models
{
    public class Case
    {
        public int Id { get; set; }

        [Required]
        public string? CaseNumber { get; set; }
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? ClientName { get; set; }
        public string? Description { get; set; }

        public DateOnly CreatedDate { get; set; }

        public string? CourtName { get; set; }

        public DateOnly? NextHearing { get; set; }
        public DateOnly? LastHearing { get; set; }

        public IEnumerable<Hearing> HearingList { get; set; } = new List<Hearing>();

        public string? LawyerId { get; set; }
    }
}
