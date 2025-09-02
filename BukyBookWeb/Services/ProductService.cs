using BukyBookWeb.Data;
using BukyBookWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BukyBookWeb.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _db;

        public ProductService(ApplicationDbContext db)
        {
            _db = db;
        }

        // Get all products with categories
        public IEnumerable<Product> GetAll()
        {
            return _db.Products.Include(p => p.Category).ToList();
        }

        // Get product by Id with category
        public Product GetById(int id)
        {
            return _db.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
        }

        // Add product
        public void Add(Product product)
        {
            _db.Products.Add(product);
            _db.SaveChanges();
        }

        // Update product
        public void Update(Product product)
        {
            _db.Products.Update(product);
            _db.SaveChanges();
        }

        // Delete product
        public void Delete(int id)
        {
            var product = _db.Products.Find(id);
            if (product != null)
            {
                _db.Products.Remove(product);
                _db.SaveChanges();
            }
        }

        // Get all categories for dropdown
        public IEnumerable<Category> GetCategories()
        {
            return _db.Categories.ToList();
        }
    }
}
