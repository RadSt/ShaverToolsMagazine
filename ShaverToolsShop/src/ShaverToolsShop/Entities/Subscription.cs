using System;
using System.Collections.Generic;

namespace ShaverToolsShop.Entities
{
    public class Subscription: BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<Product> Products { get; set; } 
    }
}