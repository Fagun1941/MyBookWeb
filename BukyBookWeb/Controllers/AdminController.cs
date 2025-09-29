using BukyBookWeb.Models;
using BukyBookWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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

        public IActionResult Users()
        {
            try
            {
                var users = _adminService.GetAllUsers();

                return View(users); 
            }
            catch (Exception ex)
            {
                return HandleError(HttpStatusCode.InternalServerError, $"Error loading users: {ex.Message}");
            }
        }

        
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

                return RedirectToAction(nameof(Users));
            }
            catch (Exception ex)
            {
                return HandleError(HttpStatusCode.InternalServerError, $"Error assigning Admin role: {ex.Message}");
            }
        }

       
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

                return RedirectToAction(nameof(Users));
            }
            catch (Exception ex)
            {
                return HandleError(HttpStatusCode.InternalServerError, $"Error removing Admin role: {ex.Message}");
            }
        }

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
