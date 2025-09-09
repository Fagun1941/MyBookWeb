using BukyBookWeb.Data;
using BukyBookWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BukyBookWeb.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext db) : base(db) { }

        public IEnumerable<Product> GetAllProduct(string search, int page, int pageSize)
        {
            return GetAll(
                search,
                page,
                pageSize,
                p => p.Title.ToLower().Contains(search.ToLower()),
                q => q.OrderBy(p => p.Id),
                p => p.Category
            );
        }

        public Product? GetByIdProduct(int id)
        {
            return _context.Products?
                           .Include(p => p.Category)
                           .FirstOrDefault(p => p.Id == id);
        }

        public void AddProduct(Product product)
        {
            Add(product);
        }
        public void UpdateProduct(Product product)
        {
            Update(product);
        }
        public void DeleteProduct(int id)
        {
            Delete(id);
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public int GetTotalProductCount(string search)
        {
            return GetTotalCount(string.IsNullOrEmpty(search)
                ? null
                : p => p.Title.Contains(search));
        }
    }
}
