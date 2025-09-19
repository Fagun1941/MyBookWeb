using BukyBookWeb.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;
using System.Diagnostics;

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
            try
            {
                _logger.LogInformation("Home page loaded");
                return View();
            }
            catch (Exception ex)
            {
                return LogAndHandleError(ex, "Error loading Home page");
            }
        }

        public IActionResult Privacy()
        {
            try
            {
                _logger.LogInformation("Privacy page loaded");
                return View();
            }
            catch (Exception ex)
            {
                return LogAndHandleError(ex, "Error loading Privacy page");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string? message = null)
        {
            var model = new CommonModel
            {
                Message = message ?? "An error occurred."
            };
            return View(model);
        }

        public IActionResult Newfeature()
        {
            try
            {
                _logger.LogInformation("Newfeature page loaded");
                return View();
            }
            catch (Exception ex)
            {
                return LogAndHandleError(ex, "Error loading Newfeature page");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetLanguage(string culture, string returnUrl = null)
        {
            try
            {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

                _logger.LogInformation("Language set to {Culture}", culture);

                return LocalRedirect(returnUrl ?? "/");
            }
            catch (Exception ex)
            {
                return LogAndHandleError(ex, $"Error setting language to {culture}");
            }
        }

        private IActionResult LogAndHandleError(Exception ex, string message)
        {
            var logGuid = Guid.NewGuid();
            using (LogContext.PushProperty("LogGuid", logGuid))
            {
                _logger.LogError(ex, "{Message} | CorrelationId={LogGuid}", message, logGuid);
            }

            TempData["ErrorMessage"] = $"Something went wrong. Tracking ID: {logGuid}";

            return RedirectToAction("Error", new { message = $"{message}. Tracking ID: {logGuid}" });
        }
    }
}
