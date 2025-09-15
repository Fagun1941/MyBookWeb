using BukyBookWeb.Models;
using BukyBookWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace BukyBookWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            var response = new CommonModel
            {
                Message = "Register page loaded",
                StatusCode = HttpStatusCode.OK,
            };
            return View();
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
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return HandleError(HttpStatusCode.BadRequest, "Registration failed", model);
            }
            catch (Exception ex)
            {
                return HandleError(HttpStatusCode.InternalServerError, $"Error during registration: {ex.Message}", model);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            var response = new CommonModel
            {
                Message = "Login page loaded",
                StatusCode = HttpStatusCode.OK,
               
            };
            return View();
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
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return HandleError(HttpStatusCode.BadRequest, "Login failed", model);
            }
            catch (Exception ex)
            {
                return HandleError(HttpStatusCode.InternalServerError, $"Error during login: {ex.Message}", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _accountService.LogoutAsync();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return HandleError(HttpStatusCode.InternalServerError, $"Error during logout: {ex.Message}");
            }
        }

        // 🔹 Centralized error handling
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
