using BukyBookWeb.Data;
using BukyBookWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BukyBookWeb.Repositories
{
    public class ProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Product> GetAll()
        {
            return _db.Products.Include(p => p.Category).ToList();
        }

        public Product GetById(int id)
        {
            return _db.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
        }

        public void Add(Product product)
        {
            _db.Products.Add(product);
            _db.SaveChanges();
        }

        public void Update(Product product)
        {
            _db.Products.Update(product);
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = _db.Products.Find(id);
            if (product != null)
            {
                _db.Products.Remove(product);
                _db.SaveChanges();
            }
        }

        public IEnumerable<Category> GetCategories()
        {
            return _db.Categories.ToList();
        }
    }
}
