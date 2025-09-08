using BukyBookWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BukyBookWeb.Repositories
{
    public interface IAdminRepository
    {
        List<ApplicationUser> GetAllUsers();
        Task AddAdminRoleAsync(ApplicationUser user);
        Task RemoveAdminRoleAsync(ApplicationUser user);
        Task<ApplicationUser?> FindByIdAsync(string userId);
    }
}
