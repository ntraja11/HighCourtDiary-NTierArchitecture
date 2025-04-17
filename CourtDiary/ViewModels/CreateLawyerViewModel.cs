using CourtDiary.Models;
using CourtDiary.Utility;
using System.ComponentModel.DataAnnotations;

namespace CourtDiary.ViewModels
{
    public class CreateLawyerViewModel
    {
        public ApplicationUser? Lawyer { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
                ErrorMessage = "Password must have at least 1 uppercase, 1 lowercase, 1 number, 1 special character, and be at least 6 characters long.")]
        public string Password { get; set; }

        public string SelectedRole { get; set; } = "";
        public List<string> AvailableRoles { get; set; } = new() { StaticDetails.RoleLawyer, StaticDetails.RoleJunior };
    }
}
