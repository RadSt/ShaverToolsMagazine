using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using ShaverToolsShop.Conventions.Repositories;
using ShaverToolsShop.Conventions.Services;
using ShaverToolsShop.Entities;
using ShaverToolsShop.Services;

namespace ShaverToolsShop.Test
{
    [TestFixture]
    public class CalculateSubscriptionSum
    {
        private Mock<ISubscriptionReadRepository> _subscriptionReadRepository;
        private ISubscriptionService _subscriptionService;
        private List<Subscription> _subscriptions;

        [SetUp]
        public void SetUp()
        {
            _subscriptionReadRepository = new Mock<ISubscriptionReadRepository>();
            _subscriptionService = new SubscriptionService(_subscriptionReadRepository.Object);
            _subscriptions = new List<Subscription>
            {
                new Subscription
                {
                    StartDate = DateTime.Parse("01/01/2017"),
                    EndDate = DateTime.Parse("03/01/2017"),
                    Products = new List<Product>
                    {
                        new Product
                        {
                            Name = "Бритвенный станок",
                            Price = 1
                        },
                        new Product
                        {
                            Name = "Средство после бритья",
                            Price = 10
                        }
                    }
                }
            };
        }
        [Test]
        public async Task ShouldReturnSubscriptions_WhenWeAskAllSubscriptions()
        {
            //Arrange
            _subscriptionReadRepository.Setup(x => x.GetAllSubscriptions()).ReturnsAsync(_subscriptions);

            //Act
            List<Subscription> results = await _subscriptionService.GetAll();

            //Assert
            Assert.IsNotNull(results);
        }
    }
}