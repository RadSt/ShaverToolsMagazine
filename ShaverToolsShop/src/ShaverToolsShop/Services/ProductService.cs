using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShaverToolsShop.Conventions.Repositories;
using ShaverToolsShop.Conventions.Services;
using ShaverToolsShop.Entities;

namespace ShaverToolsShop.Services
{
    public class ProductService : BaseService<Product>, IProductService
    {
        private readonly IProductReadRepository _productReadRepository;

        public ProductService(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<IEnumerable<SelectListItem>> GetAllForSelect()
        {
            var products = await _productReadRepository.GetAllProducts();
            var productsSelectListItems = products.Select(x =>
                new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                });

            return new SelectList(productsSelectListItems, "Value", "Text");
        }
    }
}