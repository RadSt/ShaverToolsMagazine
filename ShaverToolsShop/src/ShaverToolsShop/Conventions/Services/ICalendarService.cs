using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShaverToolsShop.Conventions.ServicesAndRepos;
using ShaverToolsShop.Entities;

namespace ShaverToolsShop.Conventions.Services
{
    public interface ICalendarService: IService<Subscription>
    {
        Task<Dictionary<DateTime, string>> GetAllSubscriptionsByPeriod(DateTime startDate, DateTime endDate);
    }
}