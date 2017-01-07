using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShaverToolsShop.Conventions.Enums;
using ShaverToolsShop.Entities;

namespace ShaverToolsShop.ViewModels
{
    public struct SubscriptionViewModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int FirstDeliveryDay { get; set; }
        public int SecondDeliveryDay { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public Guid ProductId { get; set; }
        public IEnumerable<SelectListItem> ProductsList { get; set; }
        public IEnumerable<SelectListItem> DaysInMonthList { get; set; }
        public List<Subscription> CurrentActiveSubscriptions { get; set; }
    }
}