using BukyBookWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
                return View(users);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading users: {ex.Message}";
                return View("Error");
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
                    TempData["ErrorMessage"] = "Invalid User ID.";
                    return RedirectToAction("Users");
                }

                await _adminService.AddAdminRoleAsync(userId);
                TempData["SuccessMessage"] = "User promoted to Admin.";
                return RedirectToAction("Users");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error assigning Admin role: {ex.Message}";
                return RedirectToAction("Users");
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
                    TempData["ErrorMessage"] = "Invalid User ID.";
                    return RedirectToAction("Users");
                }

                await _adminService.RemoveAdminRoleAsync(userId);
                TempData["SuccessMessage"] = "Admin role removed successfully.";
                return RedirectToAction("Users");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error removing Admin role: {ex.Message}";
                return RedirectToAction("Users");
            }
        }
    }
}
