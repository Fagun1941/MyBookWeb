using BukyBookWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BukyBookWeb.IRepository
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAuthorAsync(string search, int page, int pageSize);
        Task<Author?> GetByIdAuthorAsync(int id);
        Task AddAuthorAsync(Author author);
        Task UpdateAuthorAsync(Author author);
        Task DeleteAuthorAsync(int id);
        Task<int> GetTotalAuthorsCountAsync(string search);
    }
}
