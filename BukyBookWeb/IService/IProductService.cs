using BukyBookWeb.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace BukyBookWeb.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProduct(string search, int page , int pageSize);
        Product GetByIdProduct(int id);
        void AddProduct(Product product, IFormFile? file);
        void UpdateProduct(Product product, IFormFile? file);
        void DeleteProduct(int id);
        IEnumerable<Category> GetCategories();
        int GetTotalCountProduct(string search);
    }
}
