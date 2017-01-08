using System;
using System.Collections.Generic;
using System.Linq;
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

        public Task<List<Subscription>> GetAllSubscriptionsWithProducts()
        {
            return GetAll().Include(x=> x.Product).ToListAsync();
        }

        public Task<List<Subscription>> GetAllSubscriptionsWithProductsByPeriod(DateTime startDate, DateTime endDate)
        {
            return GetAll().Include(x => x.Product).Where(x =>
                (x.StartDate >= startDate
                && x.StartDate < endDate)
                && (x.EndDate > endDate
                || x.EndDate == null)).ToListAsync();
        }
        
    }
}