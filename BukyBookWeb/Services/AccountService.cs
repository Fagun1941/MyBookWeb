using BukyBookWeb.Models;
using BukyBookWeb.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace BukyBookWeb.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;

        public AccountService(IAccountRepository repository)
        {
            _repository = repository;
        }

        // Register user with business logic
        public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
        {
            try
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name
                };

                // You can add extra logic here (send email, assign role, etc.)
                return await _repository.RegisterUserAsync(user, model.Password);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering user {model.Email}: {ex.Message}");
                throw;
            }
        }

        // Sign in user
        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            try
            {
                return await _repository.PasswordSignInAsync(model.Email, model.Password, model.RememberMe);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging in user {model.Email}: {ex.Message}");
                throw;
            }
        }

        // Sign out user
        public async Task LogoutAsync()
        {
            try
            {
                await _repository.SignOutAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error signing out user: {ex.Message}");
                throw;
            }
        }
    }
}
