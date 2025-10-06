using BukyBookWeb.IRepository;
using BukyBookWeb.IService;
using BukyBookWeb.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BukyBookWeb.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _repository;
        private readonly ICacheService _cacheService;
        private readonly IAuditService _auditService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorService(
            IAuthorRepository repository,
            ICacheService cacheService,
            IAuditService auditService,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _cacheService = cacheService;
            _auditService = auditService;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user?.Identity?.Name ?? "Unknown";
        }

        public async Task<IEnumerable<Author>> GetAllAuthorAsync(string? search, int page, int pageSize)
        {
            try
            {
                string key = $"Author_{search}_{page}_{pageSize}";

                var cachedData = await _cacheService.GetAsync<IEnumerable<Author>>(key);
                if (cachedData != null)
                    return cachedData;

                var result = await _repository.GetAllAuthorAsync(search!, page, pageSize);
                await _cacheService.SetAsync(key, result, TimeSpan.FromMinutes(2));

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching authors: {ex.Message}");
                throw;
            }
        }

        public async Task<Author?> GetByIdAuthorAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAuthorAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching Author Id={id}: {ex.Message}");
                throw;
            }
        }

        public async Task AddAuthorAsync(Author author)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(author.AuthorName))
                    throw new ArgumentException("Author name cannot be empty.");

                await _repository.AddAuthorAsync(author);
                await _cacheService.RemoveByPrefixAsync("Author");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding Author '{author.AuthorName}': {ex.Message}");
                throw;
            }
        }

        public async Task UpdateAuthorAsync(Author author)
        {
            try
            {
                await _repository.UpdateAuthorAsync(author);
                await _cacheService.RemoveByPrefixAsync("Author");
                await _auditService.AddAsync(new AuditLog
                {
                    EnityName = "Author",
                    EntityId = author.AuthorId,
                    Action = "Update",
                    UserName = GetCurrentUser(),
                    TimeAction = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Author Id={author.AuthorId}: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteAuthorAsync(int id)
        {
            try
            {
                await _repository.DeleteAuthorAsync(id);
                await _cacheService.RemoveByPrefixAsync("Author");
                await _auditService.AddAsync(new AuditLog
                {
                    EnityName = "Author",
                    EntityId = id,
                    Action = "Delete",
                    UserName = GetCurrentUser(),
                    TimeAction = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting Author Id={id}: {ex.Message}");
                throw;
            }
        }

        public async Task<int> GetTotalCountAsync(string? search)
        {
            try
            {
                return await _repository.GetTotalAuthorsCountAsync(search);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error counting authors: {ex.Message}");
                throw;
            }
        }
    }
}
