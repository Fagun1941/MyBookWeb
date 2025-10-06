using BukyBookWeb.Models;

namespace BukyBookWeb.Services
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAllAuthorAsync(string? search, int page, int pageSize);
        Task<int> GetTotalCountAsync(string? search);
        Task<Author?> GetByIdAuthorAsync(int id);
        Task AddAuthorAsync(Author author);
        Task UpdateAuthorAsync(Author author);
        Task DeleteAuthorAsync(int id);
    }
}
