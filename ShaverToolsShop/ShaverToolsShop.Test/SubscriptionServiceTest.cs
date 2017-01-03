using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private Mock<ISubscriptionRepository> _subscriptionRepository;
        private ISubscriptionService _subscriptionService;
        private List<Subscription> _subscriptions;
        private Subscription _subscription;


        [SetUp]
        public void SetUp()
        {
            _subscriptionReadRepository = new Mock<ISubscriptionReadRepository>();
            _subscriptionRepository = new Mock<ISubscriptionRepository>();
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
            _subscription =
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
        [Test]
        public void ShouldAddSubscription_WhenWeAddSubscription()
        {
            //Arrange
            var id = Guid.NewGuid();
            _subscriptionRepository.Setup(m => m.Add(_subscription)).ReturnsAsync((Subscription e) =>
            {
                e.Id = id;
                return e;
            });

            //Act
            _subscriptionService.Create(_subscriptions);

            //Assert
            Assert.AreEqual(id, _subscription.Id);
        }
    }
}