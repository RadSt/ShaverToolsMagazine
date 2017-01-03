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

        public SubscriptionService(ISubscriptionReadRepository subscriptionReadRepository)
        {
            _subscriptionReadRepository = subscriptionReadRepository;
        }

        public async Task<List<Subscription>> GetAll()
        {
            return await _subscriptionReadRepository
                .GetAllSubscriptions();
        }
    }
}