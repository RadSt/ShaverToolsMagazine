using System;
using ShaverToolsShop.Conventions;

namespace ShaverToolsShop.Entities
{
    public class BaseEntity : IEntity
    {
        public Guid Id { get; set; }
    }
}