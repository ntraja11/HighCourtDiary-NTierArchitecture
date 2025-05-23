﻿using CourtDiary.Domain.Models;

namespace CourtDiary.Shared.ViewModels
{
    public class OrganizationAdminViewModel
    {
        public Organization? Organization { get; set; }
        public ApplicationUser? OrganizationAdmin { get; set; }

        public IEnumerable<ApplicationUser> Lawyers { get; set; } = new List<ApplicationUser>();

        public IEnumerable<Case> Cases { get; set; } = new List<Case>();
    }
}
