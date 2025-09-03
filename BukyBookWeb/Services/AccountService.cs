using BukyBookWeb.Models;
using BukyBookWeb.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BukyBookWeb.Services
{
    public class AccountService
    {
        private readonly AccountRepository _repository;

        public AccountService(AccountRepository repository)
        {
            _repository = repository;
        }

        // Register user with business logic
        public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name
            };

            // You can add extra logic here if needed (e.g., send email, assign role, etc.)
            return await _repository.RegisterUserAsync(user, model.Password);
        }

        // Sign in user
        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _repository.PasswordSignInAsync(model.Email, model.Password, model.RememberMe);
        }

        // Sign out user
        public async Task LogoutAsync()
        {
            await _repository.SignOutAsync();
        }
    }
}
