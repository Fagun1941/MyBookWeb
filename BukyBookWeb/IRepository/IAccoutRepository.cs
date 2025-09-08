using BukyBookWeb.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BukyBookWeb.Repositories
{
    public interface IAccountRepository
    {
        Task<IdentityResult> RegisterUserAsync(ApplicationUser user, string password);
        Task<SignInResult> PasswordSignInAsync(string email, string password, bool rememberMe);
        Task SignOutAsync();
    }
}
