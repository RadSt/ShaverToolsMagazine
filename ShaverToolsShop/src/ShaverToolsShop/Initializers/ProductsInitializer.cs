using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShaverToolsShop.Conventions;
using ShaverToolsShop.Entities;

namespace ShaverToolsShop.Initializers
{
    public class ProductsInitializer : IInitializable
    {
        private readonly DbContext _dbContext;

        public ProductsInitializer(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Order => 2;
        public void Initialize()
        {
            var productsNamesInDb = _dbContext.Set<Product>().Select(x => x.Name).ToList();
            var products = new List<Product>
            {
                new Product
                {
                    Name = "Бритвенный станок",
                    Price = 1
                },
                new Product
                {
                    Name = "Гель для бритья",
                    Price = 8
                },
                new Product
                {
                    Name = "Средство после бритья",
                    Price = 10
                }
            };
            var productsToAdd = products.Where(x => !productsNamesInDb.Contains(x.Name)).ToList();

            _dbContext.Set<Product>().AddRange(productsToAdd);
            _dbContext.SaveChanges();
        }
    }
}