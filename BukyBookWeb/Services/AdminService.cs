using BukyBookWeb.Models;
using BukyBookWeb.Repositories;
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
            return _repository.GetAllUsers();
        }

        // Make user Admin
        public async Task AddAdminRoleAsync(string userId)
        {
            var user = await _repository.FindByIdAsync(userId);
            if (user != null)
            {
                await _repository.AddAdminRoleAsync(user);
            }
        }

        // Remove Admin role
        public async Task RemoveAdminRoleAsync(string userId)
        {
            var user = await _repository.FindByIdAsync(userId);
            if (user != null)
            {
                await _repository.RemoveAdminRoleAsync(user);
            }
        }
    }
}
