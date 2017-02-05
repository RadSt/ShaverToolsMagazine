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
using ShaverToolsShop.ViewModels;

namespace ShaverToolsShop.Test
{
    public class CalendarServiceTest
    {
        private Mock<ISubscriptionReadRepository> _subscriptionReadRepository;
        private ICalendarService _calendarService;


        [SetUp]
        public void SetUp()
        {
            _subscriptionReadRepository = new Mock<ISubscriptionReadRepository>();
            _calendarService = new CalendarService(_subscriptionReadRepository.Object);           
        }

        [Test]
        public async Task ShouldReturnSubscriptionWithProductsByDaysForPeriod_WhenWeAskSubscriptionsWithProductsByDaysForPeriod()
        {
            //Arrange
            var startDate = DateTime.ParseExact("01.01.2017", "dd.MM.yyyy", null);
            var endDate = DateTime.ParseExact("01.02.2017", "dd.MM.yyyy", null);
            var subscriptionsForPeriod = new Dictionary<DateTime, string>
            {
                { DateTime.ParseExact("15.01.2017", "dd.MM.yyyy", null), "Бритвенный станок" }
            };
            var subscriptions = new List<Subscription>
            {
                new Subscription
                {
                    Id = Guid.NewGuid(),
                    StartDate = DateTime.ParseExact("01.01.2017", "dd.MM.yyyy", null),
                    EndDate = DateTime.ParseExact("01.02.2017", "dd.MM.yyyy", null),
                    SubscriptionType = SubscriptionType.OnceInMonth,
                    FirstDeliveryDay = 15,
                    Product =
                        new Product
                        {
                            Name = "Бритвенный станок",
                            Price = 1
                        }
                }
            };
            SetSubscriptionReadRepository(startDate, endDate, subscriptions);


            //Act
            var result = await _calendarService.GetAllSubscriptionsByPeriod(startDate, endDate);

            //Assert
            Assert.AreEqual(subscriptionsForPeriod, result);
        }

        [Test]
        public async Task ShouldReturnDaysWithProductNameFromPeriod_WhenWeAskGetSubscriptionCalendarForMonth()
        {
            //Arrange
            var startDate = DateTime.ParseExact("01.01.2017", "dd.MM.yyyy", null);
            var daysInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);
            var endDate = startDate.AddDays(daysInMonth);
            var subscriptions = new List<Subscription>
            {
                new Subscription
                {
                    Id = Guid.NewGuid(),
                    StartDate = DateTime.ParseExact("01.01.2017", "dd.MM.yyyy", null),
                    EndDate = DateTime.ParseExact("01.02.2017", "dd.MM.yyyy", null),
                    SubscriptionType = SubscriptionType.OnceInMonth,
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
                    SubscriptionType = SubscriptionType.OnceInMonth,
                    FirstDeliveryDay = 25,
                    Product =
                        new Product
                        {
                            Name = "Бритвенный станок",
                            Price = 1
                        }
                }
            };
            var result = false;
            SetSubscriptionReadRepository(startDate, endDate, subscriptions);


            //Act
            var subscriptionDays = await _calendarService.GetSubscriptionMonthCalendar(startDate);
            foreach (var week in subscriptionDays.WeeksData)
            {
                foreach (var day in week)
                {
                    if (subscriptions.Any(x => x.Product.Name == day.ProductName))
                        result = true;
                }
            }


            //Assert
            Assert.AreEqual(true, result);
        }

        private void SetSubscriptionReadRepository(DateTime startDate, DateTime endDate, List<Subscription> subscriptions)
        {
            _subscriptionReadRepository.Setup(m => m.GetAllSubscriptionsWithProductsByPeriod(startDate, endDate))
                .ReturnsAsync(subscriptions);
        }
    }
}