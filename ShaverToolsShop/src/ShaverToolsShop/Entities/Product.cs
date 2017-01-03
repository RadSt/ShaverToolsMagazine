using System;
using ShaverToolsShop.Conventions;
using ShaverToolsShop.Conventions.Enums;

namespace ShaverToolsShop.Entities
{
    public class Product: BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }   
    }
}