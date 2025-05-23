﻿using CourtDiary.Domain.Models;

namespace CourtDiary.Shared.ViewModels
{
    public class CaseDetailsViewModel
    {
        public Case? Case { get; set; }
        public IEnumerable<Hearing> HearingList { get; set; } = new List<Hearing>();
    }
}
