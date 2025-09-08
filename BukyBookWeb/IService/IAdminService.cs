using BukyBookWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BukyBookWeb.Services
{
    public interface IAdminService
    {
        List<ApplicationUser> GetAllUsers();
        Task AddAdminRoleAsync(string userId);
        Task RemoveAdminRoleAsync(string userId);
    }
}
