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
            var subscriptionViewModel = await GetSubscriptionViewModel();
            subscriptionViewModel.SubscriptionType = SubscriptionType.OnceInMonth;
            return View(subscriptionViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> AddNewSubscription(SubscriptionViewModel subscriptionViewModel,
            string startDate)
        {
            if (!ModelState.IsValid) return View("Index", subscriptionViewModel);

            var subscription = new Subscription
            {
                StartDate = DateTime.ParseExact(startDate, "dd.MM.yyyy", null),
                ProductId = subscriptionViewModel.ProductId,
                SubscriptionType = subscriptionViewModel.SubscriptionType,
                FirstDeliveryDay = subscriptionViewModel.FirstDeliveryDay,
                SecondDeliveryDay = subscriptionViewModel.SecondDeliveryDay
            };
            await _subscriptionService.AddNewSubscription(subscription);
            subscriptionViewModel = await GetSubscriptionViewModel();
            subscriptionViewModel.CurrentActiveSubscriptions = await _subscriptionService.GetAllWithProducts();
            return View("Index", subscriptionViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> CalculateSubscriptions(SubscriptionViewModel subscriptionViewModel)
        {
            if (!ModelState.IsValid) return View("Index", subscriptionViewModel);
            var newSubscriptionViewModel = await GetSubscriptionViewModel();

            if (subscriptionViewModel.CalculateDate != null)
                newSubscriptionViewModel.SubscriptionPrice
                    =
                    await _subscriptionService.CalculateSubscriptionsCost(
                        DateTime.ParseExact(subscriptionViewModel.CalculateDate, "dd.MM.yyyy", null));
            return View("Index", newSubscriptionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> StopSubscriptions(string endDate, Guid subscriptionId)
        {
            await _subscriptionService.StoppedSubscription(subscriptionId,
                DateTime.ParseExact(endDate, "dd.MM.yyyy", null));

            var newSubscriptionViewModel = await GetSubscriptionViewModel();
            return View("Index", newSubscriptionViewModel);
        }

        [NonAction]
        private async Task<SubscriptionViewModel> GetSubscriptionViewModel()
        {
            var subscriptionViewModel = new SubscriptionViewModel
            {
                CurrentActiveSubscriptions = await _subscriptionService.GetAllWithProducts(),
                ProductsList = await _productService.GetAllForSelect(),
                DaysInMonthList = _subscriptionService.GetDaysInMonthSelectList(),
                SubscriptionType = SubscriptionType.OnceInMonth,
                CalculateDate = DateTime.Now.ToString("dd.MM.yyyy")
            };
            return subscriptionViewModel;
        }
    }
}