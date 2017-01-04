using System;
using System.Collections.Generic;
using ShaverToolsShop.Conventions.Enums;

namespace ShaverToolsShop.Entities
{
    public class Subscription: BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SubscriptionStatus SubscriptionStatus { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public List<Product> Products { get; set; } 
    }
}