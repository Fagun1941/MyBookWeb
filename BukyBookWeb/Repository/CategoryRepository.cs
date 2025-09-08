using BukyBookWeb.Data;
using BukyBookWeb.Models;

namespace BukyBookWeb.Repositories
{
    public class CategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAllCategory()
        {
            return _context.Categories.ToList();
            //return DummyData.Categories;

        }

        public Category GetByIdCategory(int id)
        {
            //return _context.Categories.Find(id);
            return DummyData.Categories.FirstOrDefault(c => c.Id == id);
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
    }
}
