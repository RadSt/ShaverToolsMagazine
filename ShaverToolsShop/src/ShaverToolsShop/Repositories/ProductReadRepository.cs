using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public Task<List<Product>> GetAllProducts()
        {
            return GetAll().ToListAsync();
        }

        public Task<Product> GetProductByName(string productName)
        {
            return GetAll().FirstOrDefaultAsync(x => x.Name == productName);
        }
    }
}