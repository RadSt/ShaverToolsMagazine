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
            _subscriptionService = new SubscriptionService(_subscriptionReadRepository.Object
                , _subscriptionRepository.Object);

            _subscriptions = new List<Subscription>
            {
                new Subscription
                {
                    Id = Guid.Parse("0f19d0bc-1965-428c-a496-7b0cfa48c072"),
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
                    Id = Guid.Parse("0f19d0bc-1965-428c-a496-7b0cfa48c073"),
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
        public async Task ShouldAddSubscription_WhenWeAddSubscription()
        {
            //Arrange
            _subscriptionRepository.Setup(m => m.AddNewSubscription(_subscription)).ReturnsAsync(_subscription);

            //Act
            var addedSubscription = await _subscriptionService.AddNewSubscription(_subscription);

            //Assert
            Assert.AreEqual(_subscription.StartDate, addedSubscription.StartDate);
        }

        [Test]
        public async Task SubscriptionStatusMustBeStarted_WhenWeAddNewSubscription()
        {
            //Arrange
            _subscriptionRepository.Setup(m => m.AddNewSubscription(_subscription)).ReturnsAsync(_subscription);

            //Act
            var addedSubscription = await _subscriptionService.AddNewSubscription(_subscription);

            //Assert
            Assert.AreEqual(SubscriptionStatus.Started, addedSubscription.SubscriptionStatus);
        }

        [Test]
        public async Task SubscriptionChangesMustBeSaved_WhenWeStopedSubscription()
        {
            //Arrange
            var stoppedDate = DateTime.Parse("02/01/2017");
            _subscriptionRepository.Setup(m => m.GetSubscriptionAsync(_subscription.Id)).ReturnsAsync(_subscription);


            //Act
            await _subscriptionService.StoppedSubscription(_subscription.Id, stoppedDate);

            //Assert
            _subscriptionRepository.Verify(m => m.SaveAsync(), Times.Once);
        }

    }
}