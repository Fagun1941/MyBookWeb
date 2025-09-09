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
        public Category? GetByIdCategory(int id)
        {
            return GetById(id);
        }
        public void AddCategory(Category category)
        {
            Add(category);
        }
        public void UpdateCategory(Category category)
        {
            Update(category);
        }
        public void DeleteCategory(int id)
        {
            Delete(id);
        }

        public int GetTotalCategoriesCount(string search)
        {
            return GetTotalCount(string.IsNullOrEmpty(search)
                ? null
                : c => c.Name.Contains(search));
        }
    }
}
