using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShaverToolsShop.Conventions.Enums;
using ShaverToolsShop.Conventions.Services;
using ShaverToolsShop.Entities;
using ShaverToolsShop.ViewModels;

namespace ShaverToolsShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ISubscriptionService _subscriptionService;

        public HomeController(ISubscriptionService subscriptionService, IProductService productService)
        {
            _subscriptionService = subscriptionService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var todayDate = DateTime.Now;
            var subscriptionViewModel = await GetSubscriptionViewModel(todayDate);
            subscriptionViewModel.SubscriptionType = SubscriptionType.OnceInMonth;
            return View(subscriptionViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> AddNewSubscription(SubscriptionViewModel subscriptionViewModel,
            string startDate)
        {
            if (!ModelState.IsValid) return View("Index", subscriptionViewModel);
            var todayDate = DateTime.ParseExact(startDate, "dd.MM.yyyy", null);

            var subscription = new Subscription
            {
                StartDate = todayDate,
                ProductId = subscriptionViewModel.ProductId,
                SubscriptionType = subscriptionViewModel.SubscriptionType,
                FirstDeliveryDay = subscriptionViewModel.FirstDeliveryDay,
                SecondDeliveryDay = subscriptionViewModel.SecondDeliveryDay
            };
            await _subscriptionService.AddNewSubscription(subscription);
            subscriptionViewModel = await GetSubscriptionViewModel(todayDate);
            return View("Index", subscriptionViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> CalculateSubscriptions(SubscriptionViewModel subscriptionViewModel)
        {
            var todayDate = DateTime.ParseExact(subscriptionViewModel.CalculateDate, "dd.MM.yyyy", null);

            if (!ModelState.IsValid) return View("Index", subscriptionViewModel);
            var newSubscriptionViewModel = await GetSubscriptionViewModel(todayDate);

            if (subscriptionViewModel.CalculateDate != null)
                newSubscriptionViewModel.SubscriptionPrice
                    =
                    await _subscriptionService.CalculateSubscriptionsCost(
                       todayDate);
            return View("Index", newSubscriptionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> StopSubscriptions(string endDate, Guid subscriptionId)
        {
            var todayDate = DateTime.ParseExact(endDate, "dd.MM.yyyy", null);

            await _subscriptionService.StoppedSubscription(subscriptionId, todayDate);

            var newSubscriptionViewModel = await GetSubscriptionViewModel(todayDate);
            return View("Index", newSubscriptionViewModel);
        }

        [NonAction]
        private async Task<SubscriptionViewModel> GetSubscriptionViewModel(DateTime todayDate)
        {
            var subscriptionViewModel = new SubscriptionViewModel
            {
                CurrentActiveSubscriptions = await _subscriptionService.GetAllWithProductsWithNotExpiredDate(todayDate),
                ProductsList = await _productService.GetAllForSelect(),
                DaysInMonthList = _subscriptionService.GetDaysInMonthSelectList(),
                SubscriptionType = SubscriptionType.OnceInMonth,
                CalculateDate = DateTime.Now.ToString("dd.MM.yyyy")
            };
            return subscriptionViewModel;
        }
    }
}