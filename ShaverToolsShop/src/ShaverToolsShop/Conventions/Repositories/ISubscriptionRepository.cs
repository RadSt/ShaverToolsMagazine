using System.Threading.Tasks;
using ShaverToolsShop.Entities;

namespace ShaverToolsShop.Conventions.Repositories
{
    public interface ISubscriptionRepository: IRepository<Subscription>
    {
        Task<Subscription> AddNewSubscription(Subscription subscription);
    }
}