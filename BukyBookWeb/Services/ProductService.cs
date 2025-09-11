using BukyBookWeb.Models;
using BukyBookWeb.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BukyBookWeb.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly string _imageFolder;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
            _imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
            if (!Directory.Exists(_imageFolder))
                Directory.CreateDirectory(_imageFolder);
        }

        public IEnumerable<Product> GetAllProduct(string search, int page, int pageSize)
        {
            try
            {
                var products = _repository.GetAllProduct(search, page, pageSize);
                if (!string.IsNullOrWhiteSpace(search))
                {
                    products = products
                        .Where(c => c.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
                }
                return products;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching products with search='{search}': {ex.Message}");
                throw;
            }
        }

        public Product GetByIdProduct(int id)
        {
            try
            {
                return _repository.GetByIdProduct(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching Product with Id={id}: {ex.Message}");
                throw;
            }
        }

        public void AddProduct(Product product, IFormFile? file)
        {
            try
            {
                HandleFileUpload(product, file);
                _repository.AddProduct(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding Product '{product.Title}': {ex.Message}");
                throw;
            }
        }

        public void UpdateProduct(Product product, IFormFile? file)
        {
            try
            {
                HandleFileUpload(product, file);
                _repository.UpdateProduct(product);
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
                _repository.DeleteProduct(id);
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
                return _repository.GetCategories();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching categories: {ex.Message}");
                throw;
            }
        }

        private void HandleFileUpload(Product product, IFormFile? file)
        {
            if (file == null || file.Length == 0) return;

            try
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(_imageFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                product.ImageUrl = "/images/products/" + fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading file for Product '{product.Title}': {ex.Message}");
                throw;
            }
        }

        public int GetTotalCountProduct(string search)
        {
            try
            {
                return _repository.GetTotalProductCount(search);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error counting products with search='{search}': {ex.Message}");
                throw;
            }
        }
    }
}
