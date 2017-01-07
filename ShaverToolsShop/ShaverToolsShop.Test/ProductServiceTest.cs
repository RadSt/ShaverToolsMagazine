using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using ShaverToolsShop.Conventions.Repositories;
using ShaverToolsShop.Conventions.Services;
using ShaverToolsShop.Entities;
using ShaverToolsShop.Services;

namespace ShaverToolsShop.Test
{
    public class ProductServiceTest
    {
        private Mock<IProductReadRepository> _productReadRepository;
        private List<Product> _products;
        private IProductService _productService;

        [SetUp]
        public void SetUp()
        {
            _productReadRepository = new Mock<IProductReadRepository>();
            _productService = new ProductService(_productReadRepository.Object);

            _products = new List<Product>
            {
                new Product
                {
                    Name = "Бритвенный станок",
                    Price = 1
                },
                new Product
                {
                    Name = "Бритвенный станок + гель для бритья",
                    Price = 9
                },
                new Product
                {
                    Name = "бритвенный станок + гель + средство после бритья",
                    Price = 19
                }
            };
        }

        [Test]
        public async Task ShouldReturnThreeProductsList_WhenWeAskAllProductsForSelect()
        {
            //Arrange
            var productsQty = 3;
            _productReadRepository.Setup(x => x.GetAllProducts()).ReturnsAsync(_products);

            //Act
            var results = await _productService.GetAllForSelect();

            //Assert
            Assert.AreEqual(productsQty, results.ToList().Count);
        }
    }
}