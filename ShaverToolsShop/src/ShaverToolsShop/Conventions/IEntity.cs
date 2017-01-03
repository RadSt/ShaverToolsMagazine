using System;

namespace ShaverToolsShop.Conventions
{
    /// <summary>
    /// Интерфейс сущности
    /// </summary>
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}