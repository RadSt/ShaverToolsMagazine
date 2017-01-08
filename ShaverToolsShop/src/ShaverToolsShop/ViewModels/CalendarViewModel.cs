using System;
using System.Collections.Generic;

namespace ShaverToolsShop.ViewModels
{
    public class CalendarViewModel
    {
        public DateTime SelectedMonth { get; set; }
        public List<List<CalendarDayModel>> WeeksData { get; set; }
    }
}