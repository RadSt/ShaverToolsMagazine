using System.Collections.Generic;
using System.Threading.Tasks;
using ShaverToolsShop.Entities;

namespace ShaverToolsShop.Conventions.Repositories
{
    public interface ISubscriptionReadRepository: IReadRepository<Subscription>
    {
        Task<List<Subscription>> GetAllSubscriptions();
    }
}