using BukyBookWeb.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BukyBookWeb.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductAsync(string? search, int page, int pageSize);
        Task<int> GetTotalCountProductAsync(string? search);
        Task<Product?> GetByIdProductAsync(int id);
        Task AddProductAsync(Product product, IFormFile? file);
        Task UpdateProductAsync(Product product, IFormFile? file);
        Task DeleteProductAsync(int id);
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<IEnumerable<Author>> GetAllAuthorsAsync();
    }
}
