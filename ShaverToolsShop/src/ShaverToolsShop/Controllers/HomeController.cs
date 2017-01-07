using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            var subscriptionViewModel = new SubscriptionViewModel
            {
                CurrentActiveSubscriptions = await _subscriptionService.GetAllWithProducts(),
                ProductsList = await _productService.GetAllForSelect(),
                DaysInMonthList = _subscriptionService.GetDaysInMonth()
            };

            return View(subscriptionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewSubscription(SubscriptionViewModel subscriptionViewModel)
        {
            if (ModelState.IsValid)
            {
                var subscription = new Subscription
                {
                    StartDate = subscriptionViewModel.StartDate, 
                    EndDate = subscriptionViewModel.EndDate,
                    ProductId = subscriptionViewModel.ProductId,
                    SubscriptionType = subscriptionViewModel.SubscriptionType,
                    FirstDeliveryDay = subscriptionViewModel.FirstDeliveryDay,
                    SecondDeliveryDay = subscriptionViewModel.SecondDeliveryDay
                };
                await _subscriptionService.AddNewSubscription(subscription);
                subscriptionViewModel.CurrentActiveSubscriptions = await _subscriptionService.GetAllWithProducts();
                return View(subscriptionViewModel);
            }

            return View(subscriptionViewModel);
        }
    }
}