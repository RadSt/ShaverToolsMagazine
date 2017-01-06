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

        public async Task<List<Subscription>> GetAll()
        {
            return await _subscriptionReadRepository
                .GetAllSubscriptions();
        }

        public async Task<Subscription> AddNewSubscription(Subscription subscription)
        {
            subscription.SubscriptionStatus = SubscriptionStatus.Started;
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
    }
}