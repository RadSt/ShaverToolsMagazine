using System;
using System.Collections.Generic;
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
    public class SubscriptionActionsTest
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

            _subscription =
                new Subscription
                {
                    Id = Guid.NewGuid(),
                    StartDate = DateTime.ParseExact("01.01.2017", "dd.MM.yyyy", null),
                    FirstDeliveryDay = 20,
                    Product =
                        new Product
                        {
                            Name = "Бритвенный станок",
                            Price = 1
                        }
                };
        }

        private Mock<ISubscriptionReadRepository> _subscriptionReadRepository;
        private Mock<ISubscriptionRepository> _subscriptionRepository;
        private Mock<IProductReadRepository> _productReadRepository;

        private ISubscriptionService _subscriptionService;
        private Subscription _subscription;

        [Test]
        public async Task SubscriptionChangesMustBeSaved_WhenWeStopedSubscription()
        {
            //Arrange
            var endDate = DateTime.ParseExact("01.03.2017", "dd.MM.yyyy", null);
            _subscriptionRepository.Setup(m => m.GetSubscriptionAsync(_subscription.Id)).ReturnsAsync(_subscription);


            //Act
            await _subscriptionService.StoppedSubscription(_subscription.Id, endDate);

            //Assert
            _subscriptionRepository.Verify(m => m.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task SubscriptionStatusMustBeStarted_WhenWeAddNewSubscription()
        {
            //Arrange
            var startDate = DateTime.ParseExact("01.01.2017", "dd.MM.yyyy", null);
            _subscriptionRepository.Setup(m => m.AddNewSubscription(_subscription)).ReturnsAsync(_subscription);

            //Act
            var addedSubscription = await _subscriptionService.AddNewSubscription(_subscription);

            //Assert
            Assert.AreEqual(SubscriptionStatus.Started, addedSubscription.SubscriptionStatus);
        }

    }
}