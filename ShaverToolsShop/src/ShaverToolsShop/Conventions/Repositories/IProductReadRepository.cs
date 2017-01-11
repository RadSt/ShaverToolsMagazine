using System.Collections.Generic;
using System.Threading.Tasks;
using ShaverToolsShop.Entities;

namespace ShaverToolsShop.Conventions.Repositories
{
    public interface IProductReadRepository: IReadRepository<Product>
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> GetProductByName(string productName);
    }
}