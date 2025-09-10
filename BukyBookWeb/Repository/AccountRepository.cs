using BukyBookWeb.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace BukyBookWeb.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Register user in Identity
        public async Task<IdentityResult> RegisterUserAsync(ApplicationUser user, string password)
        {
            try
            {
                return await _userManager.CreateAsync(user, password);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering user {user?.UserName}: {ex.Message}");
                throw;
            }
        }

        // Sign in user
        public async Task<SignInResult> PasswordSignInAsync(string email, string password, bool rememberMe)
        {
            try
            {
                return await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error signing in user with email={email}: {ex.Message}");
                throw;
            }
        }

        // Sign out user
        public async Task SignOutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error signing out user: {ex.Message}");
                throw;
            }
        }
    }
}
