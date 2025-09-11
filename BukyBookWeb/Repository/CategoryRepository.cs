using BukyBookWeb.Data;
using BukyBookWeb.IRepository;
using BukyBookWeb.Models;
using System.Linq.Expressions;

namespace BukyBookWeb.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context) { }

        public IEnumerable<Category> GetAllCategory(string? search, int page, int pageSize)
        {
            try
            {
                var term = search?.Trim();
                Expression<Func<Category, bool>>? predicate =
                    string.IsNullOrEmpty(term) ? null : c => c.Name.ToLower().Contains(term.ToLower());

                return GetAll(
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
        public Category? GetByIdCategory(int id)
        {
            try
            {
                return GetById(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching Category with Id={id}: {ex.Message}");
                throw;
            }
        }

        public void AddCategory(Category category)
        {
            try
            {
                Add(category);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding Category {category.Name}: {ex.Message}");
                throw;
            }
        }

        public void UpdateCategory(Category category)
        {
            try
            {
                Update(category);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Category Id={category.Id}: {ex.Message}");
                throw;
            }
        }

        public void DeleteCategory(int id)
        {
            try
            {
                Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting Category Id={id}: {ex.Message}");
                throw;
            }
        }

        public int GetTotalCategoriesCount(string search)
        {
            try
            {
                return GetTotalCount(string.IsNullOrEmpty(search)
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
