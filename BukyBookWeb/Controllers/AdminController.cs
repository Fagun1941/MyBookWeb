using BukyBookWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BukyBookWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        // List all users
        public IActionResult Users()
        {
            var users = _adminService.GetAllUsers();
            return View(users);
        }

        // Make user Admin
        [HttpPost]
        public async Task<IActionResult> MakeAdmin(string userId)
        {
            await _adminService.AddAdminRoleAsync(userId);
            return RedirectToAction("Users");
        }

        // Remove Admin role
        [HttpPost]
        public async Task<IActionResult> RemoveAdmin(string userId)
        {
            await _adminService.RemoveAdminRoleAsync(userId);
            return RedirectToAction("Users");
        }
    }
}
