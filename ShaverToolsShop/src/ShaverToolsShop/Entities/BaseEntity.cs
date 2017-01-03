using System;
using System.ComponentModel.DataAnnotations;
using ShaverToolsShop.Conventions;

namespace ShaverToolsShop.Entities
{
    public abstract class BaseEntity: IEntity
    {
        public Guid Id { get; set; }
    }
}