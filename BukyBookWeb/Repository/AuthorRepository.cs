using BukyBookWeb.Data;
using BukyBookWeb.IRepository;
using BukyBookWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BukyBookWeb.Repositories
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Author>> GetAllAuthorAsync(string? search, int page, int pageSize)
        {
            try
            {
                var term = search?.Trim();
                Expression<Func<Author, bool>>? predicate =
                    string.IsNullOrEmpty(term) ? null : a => a.AuthorName.ToLower().Contains(term.ToLower());

                return await GetAllAsync(
                    term,
                    page,
                    pageSize,
                    predicate,
                    q => q.OrderBy(a => a.AuthorId)
                );
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
                return await GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching Author with Id={id}: {ex.Message}");
                throw;
            }
        }

        public async Task AddAuthorAsync(Author author)
        {
            try
            {
                await AddAsync(author);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding Author {author.AuthorId}: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateAuthorAsync(Author author)
        {
            try
            {
                await UpdateAsync(author);
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
                await DeleteAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting Author Id={id}: {ex.Message}");
                throw;
            }
        }

        public async Task<int> GetTotalAuthorsCountAsync(string search)
        {
            try
            {
                return await GetTotalCountAsync(string.IsNullOrEmpty(search)
                    ? null
                    : a => a.AuthorName.Contains(search));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error counting authors with search='{search}': {ex.Message}");
                throw;
            }
        }
    }
}
