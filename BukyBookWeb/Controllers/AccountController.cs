using BukyBookWeb.Models;
using BukyBookWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Serilog.Context;
using System.Net;
using System.Threading.Tasks;

namespace BukyBookWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IAccountService accountService,
            IStringLocalizer<AccountController> localizer,
            ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Register()
        {
            try
            {
                _logger.LogInformation("Register page loaded");
                return View();
            }
            catch (Exception ex)
            {
                return LogAndHandleError(ex, "Error loading Register page");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return HandleError(HttpStatusCode.BadRequest, "Invalid registration data", model);

                var result = await _accountService.RegisterAsync(model);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User registered successfully: {Email}", model.Email);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return HandleError(HttpStatusCode.BadRequest, "Registration failed", model);
            }
            catch (Exception ex)
            {
                return LogAndHandleError(ex, "Error during registration", model);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            try
            {
                _logger.LogInformation("Login page loaded");
                return View();
            }
            catch (Exception ex)
            {
                return LogAndHandleError(ex, "Error loading Login page");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return HandleError(HttpStatusCode.BadRequest, "Invalid login data", model);

                var result = await _accountService.LoginAsync(model);

                if (result.Succeeded)
                {
                    TempData["ToastMessage"] = _localizer["LoginSuccessMessage"].Value;
                    _logger.LogInformation("User logged in successfully: {Email}", model.Email);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return HandleError(HttpStatusCode.BadRequest, "Login failed", model);
            }
            catch (Exception ex)
            {
                return LogAndHandleError(ex, "Error during login", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _accountService.LogoutAsync();
                _logger.LogInformation("User logged out");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return LogAndHandleError(ex, "Error during logout");
            }
        }

        private IActionResult LogAndHandleError(Exception ex, string message, object? data = null)
        {
            var logGuid = Guid.NewGuid();
            using (LogContext.PushProperty("LogGuid", logGuid))
            {
                _logger.LogError(ex, "{Message} | CorrelationId={LogGuid}", message, logGuid);
            }

            TempData["ErrorMessage"] = $"Something went wrong. Tracking ID: {logGuid}";

            return HandleError(HttpStatusCode.InternalServerError, $"{message}. Tracking ID: {logGuid}", data);
        }

        private IActionResult HandleError(HttpStatusCode statusCode, string message, object? data = null)
        {
            var errorModel = new CommonModel
            {
                Message = message,
                StatusCode = statusCode,
                Data = data
            };
            Response.StatusCode = (int)statusCode;
            return View("Error", errorModel);
        }
    }
}
