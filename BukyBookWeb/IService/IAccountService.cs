using BukyBookWeb.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BukyBookWeb.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterViewModel model);
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
    }
}
