﻿using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShaverToolsShop.Conventions.Repositories;
using ShaverToolsShop.Entities;

namespace ShaverToolsShop.Repositories
{
    public class  SubscriptionRepository: GenericRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(DbContext context) : base(context)
        {
        }

        public async Task<Subscription> AddNewSubscription(Subscription subscription)
        {
            var subscriptionEntity =  Add(subscription);
            return subscriptionEntity;
        }

        public async Task<Subscription> GetSubscriptionAsync(Guid subscriptionId)
        {
            return await GetAsync(subscriptionId);
        }
    }
}