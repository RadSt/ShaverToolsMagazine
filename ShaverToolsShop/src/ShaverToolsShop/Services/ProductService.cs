using System.Collections.Generic;
using System.Threading.Tasks;
using ShaverToolsShop.Conventions.Repositories;
using ShaverToolsShop.Conventions.Services;
using ShaverToolsShop.Entities;

namespace ShaverToolsShop.Services
{
    public class ProductService: BaseService<Product>, IProductService
    {
        private readonly IProductReadRepository _productReadRepository;

        public ProductService(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<List<Product>> GetAll()
        {
            return await _productReadRepository.GetAllProducts();
        }
    }
}