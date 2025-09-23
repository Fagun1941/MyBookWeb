using BukyBookWeb.Helpers;
using BukyBookWeb.Models;
using BukyBookWeb.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
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
        //private readonly IMemoryCache _cache;
        private readonly ICacheService _cacheService;

        public ProductService(IProductRepository repository, /*IMemoryCache  cache , */ ICacheService cacheService)
        {
            _repository = repository;
            _cacheService = cacheService;
            //_cache = cache;

            _imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
            if (!Directory.Exists(_imageFolder))
                Directory.CreateDirectory(_imageFolder);
        }

        public IEnumerable<Product> GetAllProduct(string search, int page, int pageSize)
        {
            try
            {

                string key = $"Product_{search}_{page}_{pageSize}";

                //return CacheHelper.GetOrSet(_cache, key,
                //    () => _repository.GetAllProduct(search, page, pageSize),
                //    absolute: TimeSpan.FromMinutes(2),
                //    sliding: TimeSpan.FromSeconds(30));


                var cacheData = _cacheService.GetAsync<IEnumerable<Product>>(key).Result;

                if(cacheData != null)
                {
                    return cacheData;
                }

                var result = _repository.GetAllProduct(search, page, pageSize);
                _cacheService.SetAsync(key, result, TimeSpan.FromMinutes(2)).Wait();

                return result;
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
                //CacheHelper.Remove(_cache, "Product");
                _cacheService.RemoveByPrefixAsync("Product").Wait();
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
                //CacheHelper.Remove(_cache, "Product_");
                _cacheService.RemoveByPrefixAsync("Product").Wait();
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
                _cacheService.RemoveByPrefixAsync("Product").Wait();
                _repository.DeleteProduct(id);
                //CacheHelper.Remove(_cache, "Product_");
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
