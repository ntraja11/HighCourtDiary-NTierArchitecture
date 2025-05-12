using Microsoft.AspNetCore.Identity;

namespace CourtDiary.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }

        public string? BarRegistrationNumber { get; set; }

        public bool IsLawyer { get; set; } = false;

        public bool IsJunior { get; set; } = false;
        public int? OrganizationId { get; set; }
        public Organization? Organization { get; set; }
    }
}
