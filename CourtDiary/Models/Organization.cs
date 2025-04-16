using System.ComponentModel.DataAnnotations;

namespace CourtDiary.Models
{
    public class Organization
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = "";
        public string? City { get; set; }

    }
}
