using BukyBookWeb.Models;
using System.Collections.Generic;

namespace BukyBookWeb.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProduct(string search, int page, int pageSize);
        Product? GetByIdProduct(int id);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
        IEnumerable<Category> GetCategories();
        int GetTotalProductCount(string search);
    }
}
