using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShaverToolsShop.Conventions.Enums;

namespace ShaverToolsShop.Entities
{
    public class Subscription: BaseEntity
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public SubscriptionStatus SubscriptionStatus { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int FirstDeliveryDay { get; set; }
        public int? SecondDeliveryDay { get; set; }
        [NotMapped]
        public bool? IsEditableField { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> ProductsList { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> DaysInMonthList { get; set; }
    }
}