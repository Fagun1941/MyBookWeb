using BukyBookWeb.Data;
using BukyBookWeb.IRepository;
using BukyBookWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BukyBookWeb.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext db) : base(db) { }

        public async Task<IEnumerable<Product>> GetAllProductAsync(string search, int page, int pageSize)
        {
            try
            {
                return await Task.Run(() =>
                    GetAllAsync(
                        search,
                        page,
                        pageSize,
                        p => p.Title.ToLower().Contains(search.ToLower()),
                        q => q.OrderBy(p => p.Id),
                        p => p.Category
                    )
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching products with search='{search}': {ex.Message}");
                throw;
            }
        }

        public async Task<Product?> GetByIdProductAsync(int id)
        {
            try
            {
                return await _context.Products?
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching Product with Id={id}: {ex.Message}");
                throw;
            }
        }

        public async Task AddProductAsync(Product product)
        {
            try
            {
                await Task.Run(() => AddAsync(product));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding Product {product.Title}: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            try
            {
                await Task.Run(() => UpdateAsync(product));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Product Id={product.Id}: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                await Task.Run(() => DeleteAsync(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting Product Id={id}: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            try
            {
                return await _context.Categories.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching categories: {ex.Message}");
                throw;
            }
        }

        public async Task<int> GetTotalProductCountAsync(string search)
        {
            try
            {
                return await Task.Run(() =>
                    GetTotalCountAsync(string.IsNullOrEmpty(search)
                        ? null
                        : p => p.Title.Contains(search))
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error counting products with search='{search}': {ex.Message}");
                throw;
            }
        }
    }
}
