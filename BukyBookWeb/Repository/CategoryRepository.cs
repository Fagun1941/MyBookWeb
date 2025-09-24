using BukyBookWeb.Data;
using BukyBookWeb.IRepository;
using BukyBookWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BukyBookWeb.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Category>> GetAllCategoryAsync(string? search, int page, int pageSize)
        {
            try
            {
                var term = search?.Trim();
                Expression<Func<Category, bool>>? predicate =
                    string.IsNullOrEmpty(term) ? null : c => c.Name.ToLower().Contains(term.ToLower());

                return await GetAllAsync(
                    term,
                    page,
                    pageSize,
                    predicate,
                    q => q.OrderBy(c => c.Id)
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching categories: {ex.Message}");
                throw;
            }
        }

        public async Task<Category?> GetByIdCategoryAsync(int id)
        {
            try
            {
                return await GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching Category with Id={id}: {ex.Message}");
                throw;
            }
        }

        public async Task AddCategoryAsync(Category category)
        {
            try
            {
                await AddAsync(category);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding Category {category.Name}: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            try
            {
                await UpdateAsync(category);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Category Id={category.Id}: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteCategoryAsync(int id)
        {
            try
            {
                await DeleteAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting Category Id={id}: {ex.Message}");
                throw;
            }
        }

        public async Task<int> GetTotalCategoriesCountAsync(string search)
        {
            try
            {
                return await GetTotalCountAsync(string.IsNullOrEmpty(search)
                    ? null
                    : c => c.Name.Contains(search));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error counting categories with search='{search}': {ex.Message}");
                throw;
            }
        }
    }
}
