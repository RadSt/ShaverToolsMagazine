using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShaverToolsShop.Conventions.ServicesAndRepos;
using ShaverToolsShop.Entities;
using ShaverToolsShop.ViewModels;

namespace ShaverToolsShop.Conventions.Services
{
    public interface ICalendarService: IService<Subscription>
    {
        Task<Dictionary<DateTime, string>> GetAllSubscriptionsByPeriod(DateTime startDate, DateTime endDate);
        Task<CalendarViewModel> GetSubscriptionMonthCalendar(DateTime startDate);
    }
}