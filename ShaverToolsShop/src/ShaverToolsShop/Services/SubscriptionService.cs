using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return await _subscriptionRepository.AddNewSubscription(subscription);
        }
    }
}