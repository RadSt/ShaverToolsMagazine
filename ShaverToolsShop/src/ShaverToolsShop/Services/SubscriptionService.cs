using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public SubscriptionService(ISubscriptionReadRepository subscriptionReadRepository
            , ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionReadRepository = subscriptionReadRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<List<Subscription>> GetAllWithProducts()
        {
            return await _subscriptionReadRepository
                .GetAllSubscriptionsWithProducts();
        }

        public async Task<Subscription> AddNewSubscription(Subscription subscription, DateTime startDate)
        {
            subscription.SubscriptionStatus = SubscriptionStatus.Started;
            subscription.EndDate = startDate;
            return await _subscriptionRepository.AddNewSubscription(subscription);
        }

        public async Task<CommandResult> StoppedSubscription(Guid subscriptionId, DateTime stoppedDate)
        {
            var subscriptionEntity = await _subscriptionRepository.GetSubscriptionAsync(subscriptionId);
            subscriptionEntity.SubscriptionStatus = SubscriptionStatus.Stopped;
            subscriptionEntity.EndDate = stoppedDate;
            await _subscriptionRepository.SaveAsync();
            return CommandResult.Success;
        }

        public List<int> GetDaysInMonth()
        {
            var daysInMonth = new List<int>();
            for (var i = 0; i <= 31; i++)
            {
                daysInMonth.Add(i);
            }

            return daysInMonth;
        }
    }
}