using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShaverToolsShop.Conventions;
using ShaverToolsShop.Conventions.Enums;
using ShaverToolsShop.Conventions.Repositories;
using ShaverToolsShop.Conventions.Services;
using ShaverToolsShop.Entities;

namespace ShaverToolsShop.Services
{
    public class SubscriptionService : BaseService<Subscription>, ISubscriptionService
    {
        private readonly ISubscriptionReadRepository _subscriptionReadRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IProductReadRepository _productReadRepository;

        public SubscriptionService(ISubscriptionReadRepository subscriptionReadRepository
            , ISubscriptionRepository subscriptionRepository
            , IProductReadRepository productReadRepository)
        {
            _subscriptionReadRepository = subscriptionReadRepository;
            _subscriptionRepository = subscriptionRepository;
            _productReadRepository = productReadRepository;
        }

        public async Task<List<Subscription>> GetAllWithProductsWithNotExpiredDate(DateTime todayDate)
        {
            var subscriptions =  await _subscriptionReadRepository
                .GetAllSubscriptionsWithProducts();

            return subscriptions
                .Where(x => 
                (x.EndDate == null || x.EndDate > todayDate) 
                && x.SubscriptionStatus != SubscriptionStatus.Stopped)
                .ToList();
        }

        public async Task<Subscription> AddNewSubscription(Subscription subscription)
        {
            subscription.SubscriptionStatus = SubscriptionStatus.Started;
            var newSubscription =  await _subscriptionRepository.AddNewSubscription(subscription);
            await _subscriptionRepository.SaveAsync();
            return newSubscription;

        }

        public async Task<CommandResult> StoppedSubscription(Guid subscriptionId, DateTime stoppedDate)
        {
            var subscriptionEntity = await _subscriptionRepository.GetSubscriptionAsync(subscriptionId);
            subscriptionEntity.SubscriptionStatus = SubscriptionStatus.Stopped;
            subscriptionEntity.EndDate = stoppedDate;
            await _subscriptionRepository.SaveAsync();
            return CommandResult.Success;
        }

        public IEnumerable<SelectListItem> GetDaysInMonthSelectList()
        {
            var daysInMonth = new List<int>();
            var day = 1;

            for (var i = 0; i < 31; i++)
            {
                daysInMonth.Add(day);
                day++;
            }

            var daysInMonthList = daysInMonth.Select(x =>
                new SelectListItem
                {
                    Value = x.ToString(),
                    Text = x.ToString()
                });

            return new SelectList(daysInMonthList, "Value", "Text");
        }

        public async Task<decimal> CalculateSubscriptionsCost(DateTime reportDate)
        {
            var subscriptions = await _subscriptionReadRepository.GetAllSubscriptionsWithProducts();
            decimal cost = 0;

            foreach (var subscription in subscriptions)
            {
                if (!subscription.StartDate.HasValue)
                    return 0;
                if (subscription.FirstDeliveryDay == 0)
                    return 0;

                if (!subscription.EndDate.HasValue)
                    subscription.EndDate = reportDate;

                switch (subscription.SubscriptionType)
                {
                    case SubscriptionType.OnceInTwoMonths:
                        cost += PassedDeliveriesQtyForOnceInTwoMonths(subscription.StartDate.Value
                            , subscription.EndDate.Value, subscription.FirstDeliveryDay) 
                            * subscription.Product.Price;
                        break;
                    case SubscriptionType.OnceInMonth:
                        cost += PassedDeliveriesQtyForOnceInMonth(subscription.StartDate.Value
                           , subscription.EndDate.Value, subscription.FirstDeliveryDay)
                           * subscription.Product.Price;
                        break;
                    case SubscriptionType.TwiceInMonth:
                        cost += PassedDeliveriesQtyForTwiceInMonth(subscription.StartDate.Value
                           , subscription.EndDate.Value, subscription.FirstDeliveryDay,
                           subscription.SecondDeliveryDay)
                           * subscription.Product.Price;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return cost;
        }
        public async Task<CommandResult> ChangeSubscription(Subscription subscription)
        {
            var subscriprionEntity = await _subscriptionRepository.GetSubscriptionAsync(subscription.Id);
            var product = await _productReadRepository.GetProduct(subscription.ProductId);

            if(subscriprionEntity == null)
                return new CommandResult(false, "Subscription Not Found");
            if (product == null)
                return new CommandResult(false, "Product Not Found");

            if (subscriprionEntity.SubscriptionStatus == SubscriptionStatus.Started)
            {
                subscriprionEntity.EndDate = DateTime.Now;
                subscriprionEntity.SubscriptionStatus = SubscriptionStatus.Stopped;
            }

            subscription.ProductId = product.Id;
            subscription.StartDate = DateTime.Now;
            subscription.SubscriptionStatus = SubscriptionStatus.Started;
            var newSubscription = await _subscriptionRepository.AddNewSubscription(subscription);

            await _subscriptionRepository.SaveAsync();

            return new CommandResult(true, "", newSubscription);
        }
        #region Private

        private int PassedDeliveriesQtyForTwiceInMonth(DateTime startDate, DateTime endDate,
           int subscriptionFirstDay, int? subscriptionSecondDay)
        {
            if (!subscriptionSecondDay.HasValue)
                return 0;

            var period = endDate - startDate;
            var daysInMonth = 30;
            var deliveriesInOneMonth = 2;

            var deliveriesQty = (period.Days / daysInMonth) * deliveriesInOneMonth;

            if (period.TotalDays - (daysInMonth * deliveriesQty + subscriptionFirstDay) >= 0)
                ++deliveriesQty;

            if (period.TotalDays - (daysInMonth * deliveriesQty + subscriptionSecondDay) >= 0)
                ++deliveriesQty;

            return deliveriesQty;
        }

        private int PassedDeliveriesQtyForOnceInMonth(DateTime startDate, DateTime endDate,
            int subscriptionFirstDay)
        {
            var period = endDate - startDate;
            var daysInMonth = 30;

            var deliveriesQty = period.Days / daysInMonth;

            if (period.TotalDays - (daysInMonth * deliveriesQty + subscriptionFirstDay) >= 0)
                ++deliveriesQty;

            return deliveriesQty;
        }

        private int PassedDeliveriesQtyForOnceInTwoMonths(DateTime startDate, DateTime endDate,
            int subscriptionFirstDay)
        {
            var period = endDate - startDate;
            var daysInTwoMonths = 60;

            var deliveriesQty = period.Days / daysInTwoMonths;

            if (period.TotalDays - (daysInTwoMonths * deliveriesQty + subscriptionFirstDay) >= 0)
                ++deliveriesQty;

            return deliveriesQty;
        }
        #endregion
    }
}