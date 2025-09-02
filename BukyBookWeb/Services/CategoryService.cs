using BukyBookWeb.Data;
using BukyBookWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BukyBookWeb.Services
{
    public class CategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService (ApplicationDbContext context)
        {
            _context = context;
        }


        public IEnumerable<Category> GetAll(string search)
        {
            var categories = string.IsNullOrEmpty(search)
            ? _context.Categories.ToList() 
            : _context.Categories.Where(c => c.Name.Contains(search)).ToList();
            return categories;
        }
        public Category GetById(int id)
        {
            return _context.Categories.Find(id);
        }
        public void Add(Category category) { 
            _context.Categories.Add(category);
            _context.SaveChanges(); 
        }
        public void Update(Category category) { 
            _context.Categories.Update(category); 
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }
    }
}
