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
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching products with search='{search}': {ex.Message}");
                throw;
            }
        }

        public Product? GetByIdProduct(int id)
        {
            try
            {
                return _context.Products?
                               .Include(p => p.Category)
                               .FirstOrDefault(p => p.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching Product with Id={id}: {ex.Message}");
                throw;
            }
        }

        public void AddProduct(Product product)
        {
            try
            {
                Add(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding Product {product.Title}: {ex.Message}");
                throw;
            }
        }

        public void UpdateProduct(Product product)
        {
            try
            {
                Update(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Product Id={product.Id}: {ex.Message}");
                throw;
            }
        }

        public void DeleteProduct(int id)
        {
            try
            {
                Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting Product Id={id}: {ex.Message}");
                throw;
            }
        }

        public IEnumerable<Category> GetCategories()
        {
            try
            {
                return _context.Categories.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching categories: {ex.Message}");
                throw;
            }
        }

        public int GetTotalProductCount(string search)
        {
            try
            {
                return GetTotalCount(string.IsNullOrEmpty(search)
                    ? null
                    : p => p.Title.Contains(search));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error counting products with search='{search}': {ex.Message}");
                throw;
            }
        }
    }
}
