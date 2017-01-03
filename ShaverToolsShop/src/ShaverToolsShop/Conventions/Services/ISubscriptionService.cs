using System.Collections.Generic;
using System.Threading.Tasks;
using ShaverToolsShop.Conventions.ServicesAndRepos;
using ShaverToolsShop.Entities;

namespace ShaverToolsShop.Conventions.Services
{
    public interface ISubscriptionService: IService<Subscription>
    {
        Task<List<Subscription>> GetAll();
    }
}