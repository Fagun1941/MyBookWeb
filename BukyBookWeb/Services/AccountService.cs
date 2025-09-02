using BukyBookWeb.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BukyBookWeb.Services
{
    public class AccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Register user
        public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name
            };

            return await _userManager.CreateAsync(user, model.Password);
        }

        // Sign in user
        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false
            );
        }

        // Sign out user
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
