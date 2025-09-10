using BukyBookWeb.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BukyBookWeb.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Get all users
        public List<ApplicationUser> GetAllUsers()
        {
            try
            {
                return _userManager.Users.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all users: {ex.Message}");
                throw;
            }
        }

        // Add Admin role to user
        public async Task AddAdminRoleAsync(ApplicationUser user)
        {
            try
            {
                if (user != null && !await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding Admin role to user {user?.UserName}: {ex.Message}");
                throw;
            }
        }

        // Remove Admin role from user
        public async Task RemoveAdminRoleAsync(ApplicationUser user)
        {
            try
            {
                if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    await _userManager.RemoveFromRoleAsync(user, "Admin");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing Admin role from user {user?.UserName}: {ex.Message}");
                throw;
            }
        }

        // Find user by Id
        public async Task<ApplicationUser?> FindByIdAsync(string userId)
        {
            try
            {
                return await _userManager.FindByIdAsync(userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error finding user by Id={userId}: {ex.Message}");
                throw;
            }
        }
    }
}
