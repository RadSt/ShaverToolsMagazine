using Microsoft.EntityFrameworkCore;
using ShaverToolsShop.Conventions.Repositories;
using ShaverToolsShop.Entities;

namespace ShaverToolsShop.Repositories
{
    public class ProductReadRepository: GenericReadRepository<Product>, IProductReadRepository

    {
        public ProductReadRepository(DbContext context) : base(context)
        {
        }
    }
}