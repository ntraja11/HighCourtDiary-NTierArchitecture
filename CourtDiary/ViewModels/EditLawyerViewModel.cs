using CourtDiary.Models;
using CourtDiary.Utility;
using System.ComponentModel.DataAnnotations;

namespace CourtDiary.ViewModels
{
    public class EditLawyerViewModel
    {
        public ApplicationUser? Lawyer { get; set; }

        [Required(ErrorMessage = "Please select a role")]
        public string? SelectedRole { get; set; } = "";
        public List<string> AvailableRoles { get; set; } = new() { StaticDetails.RoleLawyer, StaticDetails.RoleJunior};
    }
}
