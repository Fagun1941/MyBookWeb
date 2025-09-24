using BukyBookWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BukyBookWeb.IRepository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductAsync(string search, int page, int pageSize);
        Task<Product?> GetByIdProductAsync(int id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<int> GetTotalProductCountAsync(string search);
    }
}
