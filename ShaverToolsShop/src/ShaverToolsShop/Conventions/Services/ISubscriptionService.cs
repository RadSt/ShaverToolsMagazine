using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShaverToolsShop.Conventions.ServicesAndRepos;
using ShaverToolsShop.Entities;

namespace ShaverToolsShop.Conventions.Services
{
    public interface ISubscriptionService: IService<Subscription>
    {
        Task<List<Subscription>> GetAllWithProducts();
        Task<Subscription> AddNewSubscription(Subscription subscription);
        Task<CommandResult> StoppedSubscription(Guid subscriptionId, DateTime stoppedDate);
        IEnumerable<SelectListItem> GetDaysInMonthSelectList();
        Task<decimal> CalculateSubscriptionsCost(DateTime reportDate);
    }
}