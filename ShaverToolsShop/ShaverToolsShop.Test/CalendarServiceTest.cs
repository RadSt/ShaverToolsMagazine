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
        private List<Subscription> _subscriptions;


        [SetUp]
        public void SetUp()
        {
            _subscriptionReadRepository = new Mock<ISubscriptionReadRepository>();
            _calendarService = new CalendarService(_subscriptionReadRepository.Object);
            _subscriptions = new List<Subscription>
            {
                new Subscription
                {
                    Id = Guid.Parse("0f19d0bc-1965-428c-a496-7b0cfa48c074"),
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
                    Id = Guid.Parse("0f19d0bc-1965-428c-a496-7b0cfa48c075"),
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
                },
                new Subscription
                {
                    Id = Guid.Parse("0f19d0bc-1965-428c-a496-7b0cfa48c076"),
                    StartDate = DateTime.ParseExact("01.01.2017", "dd.MM.yyyy", null),
                    SubscriptionType = SubscriptionType.OnceInMonth,
                    FirstDeliveryDay = 15,
                    Product =
                        new Product
                        {
                            Name = "Бритвенный станок",
                            Price = 1
                        }
                },
            };
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

             _subscriptionReadRepository.Setup(m => m.GetAllSubscriptionsWithProductsByPeriod(startDate, endDate))
                .ReturnsAsync(_subscriptions.Where(x =>
                (x.StartDate >= startDate
                && x.StartDate < endDate)
                && (x.EndDate > endDate
                || x.EndDate == null)).ToList());


            //Act
            var result = await _calendarService.GetAllSubscriptionsByPeriod(startDate, endDate);

            //Assert
            Assert.AreEqual(subscriptionsForPeriod, result);
        }
        [Test]
        public async Task ShouldReturnDaysInCurrentMonthsWithSubscriptionForMonth_WhenWeAskGetSubscriptionCalendarForMonth()
        {
            //Arrange
            var startDate = DateTime.ParseExact("01.01.2017", "dd.MM.yyyy", null);
            var endDate = new DateTime(startDate.Year, startDate.AddMonths(1).Month, 1);

            var subscriptionMonthCalendar = new CalendarViewModel();
          

            _subscriptionReadRepository.Setup(m => m.GetAllSubscriptionsWithProductsByPeriod(startDate, endDate))
               .ReturnsAsync(_subscriptions.Where(x =>
               (x.StartDate >= startDate
               && x.StartDate < endDate)
               && (x.EndDate > endDate
               || x.EndDate == null)).ToList());


            //Act
            var result = await _calendarService.GetSubscriptionMonthCalendar(startDate, endDate);

            //Assert
            Assert.AreEqual(subscriptionMonthCalendar, result);
        }
    }
}