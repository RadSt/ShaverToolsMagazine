using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using ShaverToolsShop.Conventions.Enums;
using ShaverToolsShop.Conventions.Repositories;
using ShaverToolsShop.Conventions.Services;
using ShaverToolsShop.Entities;
using ShaverToolsShop.Services;

namespace ShaverToolsShop.Test
{
    [TestFixture]
    public class SubscriptionCalculateTest
    {
        [SetUp]
        public void SetUp()
        {
            _subscriptionReadRepository = new Mock<ISubscriptionReadRepository>();
            _subscriptionRepository = new Mock<ISubscriptionRepository>();
            _productReadRepository = new Mock<IProductReadRepository>();

            _subscriptionService = new SubscriptionService(_subscriptionReadRepository.Object
                , _subscriptionRepository.Object
                , _productReadRepository.Object);

           _subscriptions = new List<Subscription>
            {
                new Subscription
                {
                    Id = Guid.NewGuid(),
                    StartDate = DateTime.ParseExact("01.01.2017", "dd.MM.yyyy", null),
                    EndDate = DateTime.ParseExact("01.02.2017", "dd.MM.yyyy", null),
                    FirstDeliveryDay = 20,
                    Product =
                        new Product
                        {
                            Name = "Бритвенный станок",
                            Price = 1
                        }
                },
                new Subscription
                {
                    Id = Guid.NewGuid(),
                    StartDate = DateTime.ParseExact("01.01.2017", "dd.MM.yyyy", null),
                    EndDate = DateTime.ParseExact("01.02.2017", "dd.MM.yyyy", null),
                    FirstDeliveryDay = 25,
                    Product =
                        new Product
                        {
                            Name = "Бритвенный станок",
                            Price = 1
                        }
                },
                new Subscription
                {
                    Id = Guid.NewGuid(),
                    StartDate = DateTime.ParseExact("01.01.2017", "dd.MM.yyyy", null),
                    FirstDeliveryDay = 15,
                    Product =
                        new Product
                        {
                            Name = "Бритвенный станок",
                            Price = 1
                        }
                }
            };
        }

        private Mock<ISubscriptionReadRepository> _subscriptionReadRepository;
        private Mock<ISubscriptionRepository> _subscriptionRepository;
        private Mock<IProductReadRepository> _productReadRepository;

        private ISubscriptionService _subscriptionService;
        private List<Subscription> _subscriptions;

        [Test]
        public async Task MustReturnOnceTimeCost_WhenWeCalculatingSubscriptionOnceATwoMonthsForTwoMonthsForOneProduct()
        {
            //Arrange
            var twoMonthsDate = DateTime.ParseExact("01.03.2017", "dd.MM.yyyy", null);
            var subscriptionCost = 3M;
            foreach (var subscription in _subscriptions)
            {
                subscription.SubscriptionType = SubscriptionType.OnceInTwoMonths;
            }
            SetSubscriptions(_subscriptions);

            //Act
            var calculatedSubscriptionCost = await _subscriptionService.CalculateSubscriptionsCost(twoMonthsDate);

            //Assert
            Assert.AreEqual(subscriptionCost, calculatedSubscriptionCost);
        }

        [Test]
        public async Task MustReturnOneTimeCost_WhenWeCalculatingSubscriptionOnceAMonthForOneMonthForOneProduct()
        {
            //Arrange
            var oneMonthDate = DateTime.ParseExact("01.02.2017", "dd.MM.yyyy", null);
            foreach (var subscription in _subscriptions)
            {
                subscription.SubscriptionType = SubscriptionType.OnceInMonth;
            }
            var subscriptionCost = 3M;
            SetSubscriptions(_subscriptions);

            //Act
            var calculatedSubscriptionCost = await _subscriptionService.CalculateSubscriptionsCost(oneMonthDate);

            //Assert
            Assert.AreEqual(subscriptionCost, calculatedSubscriptionCost);
        }

        [Test]
        public async Task MustReturnTwiceTimeCost_WhenWeCalculatingSubscriptionTwiceAMonthForOneMonthForOneProduct()
        {
            //Arrange
            var oneMonthDate = DateTime.ParseExact("01.02.2017", "dd.MM.yyyy", null);
            foreach (var subscription in _subscriptions)
            {
                subscription.SubscriptionType = SubscriptionType.TwiceInMonth;
                subscription.SecondDeliveryDay = 28;
            }
            var subscriptionCost = 6M;
            SetSubscriptions(_subscriptions);

            //Act
            var calculatedSubscriptionCost = await _subscriptionService.CalculateSubscriptionsCost(oneMonthDate);

            //Assert
            Assert.AreEqual(subscriptionCost, calculatedSubscriptionCost);
        }

        private void SetSubscriptions(List<Subscription> subscriptions)
        {
            _subscriptionReadRepository.Setup(m => m.GetAllSubscriptionsWithProducts()).ReturnsAsync(subscriptions);
        }
    }
}
