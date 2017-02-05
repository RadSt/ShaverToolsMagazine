using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using ShaverToolsShop.Conventions;
using ShaverToolsShop.Conventions.Enums;
using ShaverToolsShop.Conventions.Repositories;
using ShaverToolsShop.Conventions.Services;
using ShaverToolsShop.Entities;
using ShaverToolsShop.Services;

namespace ShaverToolsShop.Test
{
    [TestFixture]
    public class SubscriptionServiceTest
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
        private Subscription _subscription;

        [Test]
        public async Task ShouldAddSubscription_WhenWeAddSubscription()
        {
            //Arrange
            var newSubscriptionHash = _subscription.GetHashCode();
            _subscriptionRepository.Setup(m => m.AddNewSubscription(_subscription))
                .ReturnsAsync(_subscription);

            //Act
            var addedSubscription = await _subscriptionService.AddNewSubscription(_subscription);
            var addedSubscriptionHash = addedSubscription.GetHashCode();

            //Assert
            Assert.AreEqual(newSubscriptionHash, addedSubscriptionHash);
        }

        [Test]
        public async Task ShouldReturnSubscriptionsWithProductsBeforeEndDate_WhenWeAskAllSubscriptionsWithProductsBeforeEndDate()
        {
            //Arrange
            var todayDate = DateTime.ParseExact("01.03.2017", "dd.MM.yyyy", null);
            var filteredByEndDateSubscriptions = _subscriptions.Where(x => x.EndDate == null || x.EndDate > todayDate).ToList();
            _subscriptionReadRepository.Setup(x => x.GetAllSubscriptionsWithProducts()).ReturnsAsync(_subscriptions);

            //Act
            var result = await _subscriptionService.GetAllWithProductsWithNotExpiredDate(todayDate);

            //Assert
            Assert.AreEqual(filteredByEndDateSubscriptions, result);
        }

        [Test]
        public async Task StartDateMustPassedDate_WhenWeAddSubscription()
        {
            //Arrange
            var startDate = DateTime.ParseExact("01.01.2017", "dd.MM.yyyy", null);
            _subscriptionRepository.Setup(m => m.AddNewSubscription(_subscription))
                .ReturnsAsync(_subscription);

            //Act
            var addedSubscription = await _subscriptionService.AddNewSubscription(_subscription);

            //Assert
            Assert.AreEqual(addedSubscription.StartDate, startDate);
        }

       

        [Test]
        public async Task WeGetRecreatedSubscriptionEntity_WhenWeUpdateSubscription()
        {
            //Arrange
           var updatedSubscription = new Subscription
           {
               Id = Guid.NewGuid(),
               StartDate = DateTime.ParseExact("01.03.2017", "dd.MM.yyyy", null),
               EndDate = DateTime.ParseExact("01.06.2017", "dd.MM.yyyy", null),
               FirstDeliveryDay = 15,
               SubscriptionStatus = SubscriptionStatus.Started,
               Product =
                        new Product
                        {
                            Name = "Бритвенный станок + гель для бритья",
                            Price = 9
                        }
           };

            var newSubscription = new Subscription
            {
                Id = Guid.NewGuid(),
                StartDate = DateTime.ParseExact("01.03.2017", "dd.MM.yyyy", null),
                EndDate = DateTime.ParseExact("01.06.2017", "dd.MM.yyyy", null),
                FirstDeliveryDay = 15,
                SubscriptionStatus = SubscriptionStatus.Started,
                Product =
                        new Product
                        {
                            Name = "Бритвенный станок + гель для бритья",
                            Price = 9
                        }
            };

            _productReadRepository.Setup(m => m.GetProduct(updatedSubscription.ProductId)).ReturnsAsync(updatedSubscription.Product);
            _subscriptionRepository.Setup(m => m.GetSubscriptionAsync(updatedSubscription.Id)).ReturnsAsync(_subscription);
            _subscriptionRepository.Setup(m => m.AddNewSubscription(updatedSubscription))
               .ReturnsAsync(newSubscription);
            var updatedSubscriptionHash = updatedSubscription.GetHashCode();


            //Act
            var result = await  _subscriptionService.ChangeSubscription(updatedSubscription);
            var resultEntity = result.Addition as Subscription;
            var resultEntityHash = resultEntity?.GetHashCode();

            //Assert
            Assert.AreNotEqual(updatedSubscriptionHash, resultEntityHash);
        }

        [Test]
        public async Task WeGetProductNotFoundMessage_WhenWeUpdateSubscriptionWithNoExistproductId()
        {
            //Arrange
            var updatedSubscription = new Subscription
            {
                Id = Guid.NewGuid(),
                StartDate = DateTime.ParseExact("01.03.2017", "dd.MM.yyyy", null),
                EndDate = DateTime.ParseExact("01.06.2017", "dd.MM.yyyy", null),
                FirstDeliveryDay = 15,
                Product =
                         new Product
                         {
                             Id = Guid.NewGuid(),
                             Name = "Бритвенный станок + гель для бритья",
                             Price = 9
                         }
            };
            _productReadRepository.Setup(m => m.GetProduct(updatedSubscription.Product.Id)).ReturnsAsync(null);
            _subscriptionRepository.Setup(m => m.GetSubscriptionAsync(updatedSubscription.Id)).ReturnsAsync(_subscription);

            //Act
            var result = await _subscriptionService.ChangeSubscription(updatedSubscription);

            //Assert
            Assert.IsTrue(result.Message == "Product Not Found");
        }

        [Test]
        public async Task WeGetSubscriptionNotFoundMessage_WhenWeUpdateSubscriptionWithNoExistproductId()
        {
            //Arrange
            var updatedSubscription = new Subscription
            {
                Id = Guid.NewGuid(),
                StartDate = DateTime.ParseExact("01.03.2017", "dd.MM.yyyy", null),
                EndDate = DateTime.ParseExact("01.06.2017", "dd.MM.yyyy", null),
                FirstDeliveryDay = 15,
                Product =
                         new Product
                         {
                             Id = Guid.NewGuid(),
                             Name = "Бритвенный станок + гель для бритья",
                             Price = 9
                         }
            };
            _productReadRepository.Setup(m => m.GetProduct(updatedSubscription.Product.Id)).ReturnsAsync(updatedSubscription.Product);
            _subscriptionRepository.Setup(m => m.GetSubscriptionAsync(updatedSubscription.Id)).ReturnsAsync(null);

            //Act
            var result = await _subscriptionService.ChangeSubscription(updatedSubscription);

            //Assert
            Assert.IsTrue(result.Message == "Subscription Not Found");
        }

    }
}