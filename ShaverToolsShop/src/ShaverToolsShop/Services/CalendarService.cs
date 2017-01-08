using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Packaging;
using ShaverToolsShop.Conventions.Enums;
using ShaverToolsShop.Conventions.Repositories;
using ShaverToolsShop.Conventions.Services;
using ShaverToolsShop.Entities;
using ShaverToolsShop.ViewModels;

namespace ShaverToolsShop.Services
{
    public class CalendarService : BaseService<Subscription>, ICalendarService
    {
        private readonly ISubscriptionReadRepository _subscriptionReadRepository;

        public CalendarService(ISubscriptionReadRepository subscriptionReadRepository)
        {
            _subscriptionReadRepository = subscriptionReadRepository;
        }

        public async Task<Dictionary<DateTime, string>> GetAllSubscriptionsByPeriod(DateTime startDate, DateTime endDate)
        {
            var subscriptions = await _subscriptionReadRepository
                .GetAllSubscriptionsWithProductsByPeriod(startDate, endDate);

            var subscriptionsByDays = new Dictionary<DateTime, string>();

            foreach (var subscription in subscriptions)
                switch (subscription.SubscriptionType)
                {
                    case SubscriptionType.OnceInTwoMonths:
                        subscriptionsByDays.AddRange(GetsubscriptionsByDaysForOnceInTwoMonths(subscription, startDate));
                        break;
                    case SubscriptionType.OnceInMonth:
                        subscriptionsByDays.AddRange(GetsubscriptionsByDaysForOnceInMonth(subscription, startDate));
                        break;
                    case SubscriptionType.TwiceInMonth:
                        subscriptionsByDays.AddRange(GetsubscriptionsByDaysForTwiceInMonth(subscription, startDate));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


            return subscriptionsByDays;
        }

        public async Task<CalendarViewModel> GetSubscriptionMonthCalendar(DateTime startDate)
        {
            var days = DateTime.DaysInMonth(startDate.Year, startDate.Month);

            var firstDateDayOfWeek = GetDayOfWeek(startDate);

            var subscriptionsByDays = await GetAllSubscriptionsByPeriod(startDate, startDate.AddDays(days));

            var weeks = new List<List<CalendarDayModel>>();

            for (int i = -firstDateDayOfWeek; i < days; i += 7)
            {
                var list = new List<CalendarDayModel>();

                for (int j = i; j < i + 7; j++)
                {
                    var calendarDateData = new CalendarDayModel();

                    // Не учитываем дни, которые не входят в учетный месяц
                    if (j >= 0 && j < days)
                    {
                        calendarDateData.Day = j + 1;

                        var date = startDate.AddDays(j);

                        if (subscriptionsByDays.ContainsKey(date))
                        {
                            calendarDateData.ProductName = subscriptionsByDays[date];
                        }
                    }

                    list.Add(calendarDateData);
                }

                weeks.Add(list);
            }

            var model = new CalendarViewModel()
            {
                SelectedMonth = startDate,
                WeeksData = weeks
            };

            return model;
        }

        private int GetDayOfWeek(DateTime dt)
        {
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Monday: return 0;
                case DayOfWeek.Tuesday: return 1;
                case DayOfWeek.Wednesday: return 2;
                case DayOfWeek.Thursday: return 3;
                case DayOfWeek.Friday: return 4;
                case DayOfWeek.Saturday: return 5;
                case DayOfWeek.Sunday: return 6;
            }

            return 0;
        }

        private Dictionary<DateTime, string> GetsubscriptionsByDaysForOnceInTwoMonths(Subscription subscription,
            DateTime startDate)
        {
            if (subscription.StartDate == null) return new Dictionary<DateTime, string>();

            var months = Math.Abs(subscription.StartDate.Value.Month - startDate.Month + 12 *
                                  (subscription.StartDate.Value.Year - startDate.Year));
            if (IsEven(months))
                return new Dictionary<DateTime, string>
                {
                    {
                        new DateTime(startDate.Year, startDate.Month, subscription.FirstDeliveryDay),
                        subscription.Product.Name
                    }
                };
            return new Dictionary<DateTime, string>();
        }

        private bool IsEven(int months)
        {
            return months % 2 == 0;
        }

        private Dictionary<DateTime, string> GetsubscriptionsByDaysForTwiceInMonth(Subscription subscription,
            DateTime startDate)
        {
            if (subscription.SecondDeliveryDay != null)
                return new Dictionary<DateTime, string>
                {
                    {
                        new DateTime(startDate.Year, startDate.Month, subscription.FirstDeliveryDay),
                        subscription.Product.Name
                    },
                    {
                        new DateTime(startDate.Year, startDate.Month, subscription.SecondDeliveryDay.Value),
                        subscription.Product.Name
                    }
                };
            return new Dictionary<DateTime, string>();
        }

        private Dictionary<DateTime, string> GetsubscriptionsByDaysForOnceInMonth(Subscription subscription,
            DateTime startDate)
        {
            return new Dictionary<DateTime, string>
            {
                {
                    new DateTime(startDate.Year, startDate.Month, subscription.FirstDeliveryDay),
                    subscription.Product.Name
                }
            };
        }
    }
}