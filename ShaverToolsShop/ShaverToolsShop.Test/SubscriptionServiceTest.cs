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
    public class CalculateSubscriptionSum
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
                    Id = Guid.Parse("0f19d0bc-1965-428c-a496-7b0cfa48c073"),
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
                    Id = Guid.Parse("0f19d0bc-1965-428c-a496-7b0cfa48c074"),
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
                    Id = Guid.Parse("0f19d0bc-1965-428c-a496-7b0cfa48c075"),
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
                    Id = Guid.Parse("0f19d0bc-1965-428c-a496-7b0cfa48c076"),
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
        public async Task MustReturnOnceTimeCost_WhenWeCalculatingSubscriptionOnceATwoMonthsForTwoMonthsForOneProduct()
        {
            //Arrange
            var twoMonthsDate = DateTime.ParseExact("01.03.2017", "dd.MM.yyyy", null);
            var subscriptionCost = 3M;
            _subscriptionReadRepository.Setup(m => m.GetAllSubscriptionsWithProducts()).ReturnsAsync(_subscriptions);
            foreach (var subscription in _subscriptions)
            {
                subscription.SubscriptionType = SubscriptionType.OnceInTwoMonths;
            }
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
            _subscriptionReadRepository.Setup(m => m.GetAllSubscriptionsWithProducts()).ReturnsAsync(_subscriptions);

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
            _subscriptionReadRepository.Setup(m => m.GetAllSubscriptionsWithProducts()).ReturnsAsync(_subscriptions);

            //Act
            var calculatedSubscriptionCost = await _subscriptionService.CalculateSubscriptionsCost(oneMonthDate);

            //Assert
            Assert.AreEqual(subscriptionCost, calculatedSubscriptionCost);
        }

        [Test]
        public async Task ShouldAddSubscription_WhenWeAddSubscription()
        {
            //Arrange
            _subscriptionRepository.Setup(m => m.AddNewSubscription(_subscription))
                .ReturnsAsync(_subscription);

            //Act
            var addedSubscription = await _subscriptionService.AddNewSubscription(_subscription);

            //Assert
            Assert.AreEqual(_subscription.Id, addedSubscription.Id);
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

        [Test]
        public void WeGetListWithFirtyOneDay_WhenWeAskDaysForSubscription()
        {
            //Arrange
            var daysInMonthList = new List<int>();
            var day = 1;
            for (var i = 0; i < 31; i++)
            {
                daysInMonthList.Add(day);
                day++;
            }

            //Act
            var newDaysInMonth = _subscriptionService.GetDaysInMonthSelectList().Select(x =>
                int.Parse(x.Value)
            ).ToList();

            //Assert
            Assert.AreEqual(daysInMonthList, newDaysInMonth);
        }

        [Test]
        public async Task WeGetUpdatesSubscriptionEntity_WhenWeUpdateSubscription()
        {
            //Arrange
           var updatedSubscription = new Subscription
           {
               Id = Guid.Parse("0f19d0bc-1965-428c-a496-7b0cfa48c073"),
               StartDate = DateTime.ParseExact("01.03.2017", "dd.MM.yyyy", null),
               EndDate = DateTime.ParseExact("01.06.2017", "dd.MM.yyyy", null),
               FirstDeliveryDay = 15,
               Product =
                        new Product
                        {
                            Id = Guid.Parse("0f19d0bc-1965-428c-a496-7b0cfa48c077"),
                            Name = "Бритвенный станок + гель для бритья",
                            Price = 9
                        }
           };
            _productReadRepository.Setup(m => m.GetProductByName(updatedSubscription.Product.Name)).ReturnsAsync(updatedSubscription.Product);
            _subscriptionRepository.Setup(m => m.GetSubscriptionAsync(updatedSubscription.Id)).ReturnsAsync(_subscription);

            //Act
            var result = await  _subscriptionService.UpdateSubscription(updatedSubscription);
            var updatedEntity = result.Addition as Subscription;
       
            //Assert
            Assert.AreEqual(updatedSubscription.FirstDeliveryDay, updatedEntity.FirstDeliveryDay);
        }

        [Test]
        public async Task WeGetProductNotFoundMessage_WhenWeUpdateSubscriptionWithNoExistproductId()
        {
            //Arrange
            var updatedSubscription = new Subscription
            {
                Id = Guid.Parse("0f19d0bc-1965-428c-a496-7b0cfa48c073"),
                StartDate = DateTime.ParseExact("01.03.2017", "dd.MM.yyyy", null),
                EndDate = DateTime.ParseExact("01.06.2017", "dd.MM.yyyy", null),
                FirstDeliveryDay = 15,
                Product =
                         new Product
                         {
                             Id = Guid.Parse("0f19d0bc-1965-428c-a496-7b0cfa48c000"),
                             Name = "Бритвенный станок + гель для бритья",
                             Price = 9
                         }
            };
            _productReadRepository.Setup(m => m.GetProductByName(updatedSubscription.Product.Name)).ReturnsAsync(null);
            _subscriptionRepository.Setup(m => m.GetSubscriptionAsync(updatedSubscription.Id)).ReturnsAsync(_subscription);

            //Act
            var result = await _subscriptionService.UpdateSubscription(updatedSubscription);

            //Assert
            Assert.IsTrue(result.Message == "Product Not Found");
        }

    }
}