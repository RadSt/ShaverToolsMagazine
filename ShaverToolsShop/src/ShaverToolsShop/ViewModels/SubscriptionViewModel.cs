using System;
using System.Collections.Generic;
using ShaverToolsShop.Conventions.Enums;
using ShaverToolsShop.Entities;

namespace ShaverToolsShop.ViewModels
{
    public struct SubscriptionViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int FirstDeliveryDay { get; set; }
        public int SecondDeliveryDay { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public List<Product> ProductsList { get; set; }
        public List<int> DaysInMonthList { get; set; }
        public List<Subscription> CurrentActiveSubscriptions { get; set; }
    }
}