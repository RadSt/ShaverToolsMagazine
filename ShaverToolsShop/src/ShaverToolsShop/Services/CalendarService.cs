using ShaverToolsShop.Conventions.Repositories;
using ShaverToolsShop.Conventions.Services;
using ShaverToolsShop.Entities;

namespace ShaverToolsShop.Services
{
    public class CalendarService: BaseService<Subscription>, ICalendarService
    {
        private readonly ISubscriptionReadRepository _subscriptionReadRepository;

        public CalendarService(ISubscriptionReadRepository subscriptionReadRepository)
        {
            _subscriptionReadRepository = subscriptionReadRepository;
        }
    }
}