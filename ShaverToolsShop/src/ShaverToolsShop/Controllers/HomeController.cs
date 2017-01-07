using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShaverToolsShop.Conventions.Services;
using ShaverToolsShop.Entities;
using ShaverToolsShop.ViewModels;

namespace ShaverToolsShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IProductService _productService;

        public HomeController(ISubscriptionService subscriptionService, IProductService productService)
        {
            _subscriptionService = subscriptionService;
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            var subsscriptionViewModel = new SubscriptionViewModel
            {
                CurrentActiveSubscriptions = await _subscriptionService.GetAllWithProducts(),
                ProductsList = await _productService.GetAll(),
                DaysInMonthList = _subscriptionService.GetDaysInMonth()
            };

            return View(subsscriptionViewModel);
        }
    }
}
