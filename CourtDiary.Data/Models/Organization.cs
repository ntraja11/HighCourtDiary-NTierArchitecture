using System.ComponentModel.DataAnnotations;

namespace CourtDiary.Data.Models
{
    public class Organization
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = "";
        public string? City { get; set; }

        public bool IsActive { get; set; } = false;

        public DateOnly? CreatedDate { get; set; }

        public DateOnly? ActivatedDate { get; set; }

        public string? CreatedBy { get; set; }
    }
}
