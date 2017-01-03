using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShaverToolsShop.Conventions.Repositories;
using ShaverToolsShop.Entities;

namespace ShaverToolsShop.Repositories
{
    public class SubscriptionReadRepository: GenericReadRepository<Subscription>, ISubscriptionReadRepository
    {
        public SubscriptionReadRepository(DbContext context) : base(context)
        {
        }

        public Task<List<Subscription>> GetAllSubscriptions()
        {
            return GetAll().ToListAsync();
        }
    }
}