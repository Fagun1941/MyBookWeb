using BukyBookWeb.Models;
using BukyBookWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BukyBookWeb.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _repository;

        public AdminService(IAdminRepository repository)
        {
            _repository = repository;
        }

        // Get all users
        public List<ApplicationUser> GetAllUsers()
        {
            try
            {
                return _repository.GetAllUsers();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all users: {ex.Message}");
                throw;
            }
        }

        // Make user Admin
        public async Task AddAdminRoleAsync(string userId)
        {
            try
            {
                var user = await _repository.FindByIdAsync(userId);
                if (user != null)
                {
                    await _repository.AddAdminRoleAsync(user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding Admin role to user Id={userId}: {ex.Message}");
                throw;
            }
        }

        // Remove Admin role
        public async Task RemoveAdminRoleAsync(string userId)
        {
            try
            {
                var user = await _repository.FindByIdAsync(userId);
                if (user != null)
                {
                    await _repository.RemoveAdminRoleAsync(user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing Admin role from user Id={userId}: {ex.Message}");
                throw;
            }
        }
    }
}
