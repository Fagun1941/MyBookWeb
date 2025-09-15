using System.Diagnostics;
using BukyBookWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BukyBookWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string? message = null)
        {
            var model = new CommonModel 
            {
                Message = message
            };

            return View(model);
        }

        public IActionResult Newfeature()
        {
            return View();
        }
    }
}
