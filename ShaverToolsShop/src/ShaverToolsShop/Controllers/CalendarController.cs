using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShaverToolsShop.Conventions.Services;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ShaverToolsShop.Controllers
{
    public class CalendarController : Controller
    {
        private readonly ICalendarService _calendarService;

        public CalendarController(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }
        public async Task<IActionResult> Index(DateTime? selectedDate)
        {
            var date = selectedDate ?? DateTime.Now;
            var todayMonth = new DateTime(date.Year, date.Month, 1);

            var subscriptionByDays = await _calendarService.GetSubscriptionMonthCalendar(todayMonth);
            subscriptionByDays.SelectedMonth = todayMonth;

            return View(subscriptionByDays);
        }
    }
}
