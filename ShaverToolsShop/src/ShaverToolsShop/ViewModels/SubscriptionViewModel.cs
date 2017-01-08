using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShaverToolsShop.Conventions.Enums;
using ShaverToolsShop.Entities;

namespace ShaverToolsShop.ViewModels
{
    public class SubscriptionViewModel
    {
        public string CalculateDate { get; set; }
        public int FirstDeliveryDay { get; set; }
        public int SecondDeliveryDay { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public Guid ProductId { get; set; }
        public decimal? SubscriptionPrice { get; set; }

        public IEnumerable<SelectListItem> ProductsList { get; set; }
        public IEnumerable<SelectListItem> DaysInMonthList { get; set; }
        public List<Subscription> CurrentActiveSubscriptions { get; set; }
    }
}