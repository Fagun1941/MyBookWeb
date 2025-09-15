using BukyBookWeb.Models;
using BukyBookWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BukyBookWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // List all users
        public IActionResult Users()
        {
            try
            {
                var users = _adminService.GetAllUsers();

                var response = new CommonModel
                {
                    Message = "Users loaded successfully",
                    StatusCode = HttpStatusCode.OK,
                    Data = users
                };

                return View(users);
            }
            catch (Exception ex)
            {
                return HandleError(HttpStatusCode.InternalServerError, $"Error loading users: {ex.Message}");
            }
        }

        // Make user Admin
        [HttpPost]
        public async Task<IActionResult> MakeAdmin(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return HandleError(HttpStatusCode.BadRequest, "Invalid User ID.");
                }

                await _adminService.AddAdminRoleAsync(userId);

                var response = new CommonModel
                {
                    Message = "User promoted to Admin successfully",
                    StatusCode = HttpStatusCode.OK
                };

                return RedirectToAction(nameof(Users));
            }
            catch (Exception ex)
            {
                return HandleError(HttpStatusCode.InternalServerError, $"Error assigning Admin role: {ex.Message}");
            }
        }

        // Remove Admin role
        [HttpPost]
        public async Task<IActionResult> RemoveAdmin(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return HandleError(HttpStatusCode.BadRequest, "Invalid User ID.");
                }

                await _adminService.RemoveAdminRoleAsync(userId);

                var response = new CommonModel
                {
                    Message = "Admin role removed successfully",
                    StatusCode = HttpStatusCode.OK
                };

                return RedirectToAction(nameof(Users));
            }
            catch (Exception ex)
            {
                return HandleError(HttpStatusCode.InternalServerError, $"Error removing Admin role: {ex.Message}");
            }
        }

        // 🔹 Centralized error handling
        private IActionResult HandleError(HttpStatusCode statusCode, string message)
        {
            var errorModel = new CommonModel
            {
                Message = message,
                StatusCode = statusCode
            };
            Response.StatusCode = (int)statusCode;
            return View("Error", errorModel);
        }
    }
}
