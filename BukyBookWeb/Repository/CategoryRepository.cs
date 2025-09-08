using BukyBookWeb.Data;
using BukyBookWeb.IRepository;
using BukyBookWeb.Models;

namespace BukyBookWeb.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAllCategory(string search,int page, int pageSize)
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Name.ToLower().Contains(search.ToLower()));
            }

            return query.OrderBy(p => p.Id)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
            //return DummyData.Categories;

        }

        public Category GetByIdCategory(int id)
        {
            return _context.Categories.Find(id);
            //return DummyData.Categories.FirstOrDefault(c => c.Id == id);
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();

            //DummyData.Categories.Add(category);
        }

        public void UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();

            //var existing = DummyData.Categories.FirstOrDefault(c => c.Id == category.Id);
            //if (existing != null)
            //{
            //    existing.Name = category.Name;
            //    existing.DisplayOrder = category.DisplayOrder;
            //}
        }

        public void DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }

            //var category = DummyData.Categories.FirstOrDefault(c => c.Id == id);
            //if (category != null)
            //{
            //    DummyData.Categories.Remove(category);
            //}
        }
        public int GetTotalCategoriesCount(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return _context.Categories.Count();
            }
            return _context.Categories
                          .Where(c => c.Name.Contains(search))
                          .Count();
        }

    }
}
