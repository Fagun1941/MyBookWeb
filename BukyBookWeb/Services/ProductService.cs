using BukyBookWeb.Helpers;
using BukyBookWeb.IRepository;
using BukyBookWeb.IService;
using BukyBookWeb.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BukyBookWeb.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly string _imageFolder;
        //private readonly IMemoryCache _cache;
        private readonly ICacheService _cacheService;
        private readonly IAuditService _auditService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user?.Identity?.Name ?? "Unknown";
        }

        public ProductService(IProductRepository repository, /*IMemoryCache  cache , */ ICacheService cacheService, IAuditService auditService, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _cacheService = cacheService;
            //_cache = cache;
            _auditService = auditService;
            _httpContextAccessor = httpContextAccessor;

            _imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
            if (!Directory.Exists(_imageFolder))
                Directory.CreateDirectory(_imageFolder);
        }

        public async Task<IEnumerable<Product>> GetAllProductAsync(string search, int page, int pageSize)
        {
            try
            {
                string key = $"Product_{search}_{page}_{pageSize}";

                //return CacheHelper.GetOrSet(_cache, key,
                //    () => _repository.GetAllProduct(search, page, pageSize),
                //    absolute: TimeSpan.FromMinutes(2),
                //    sliding: TimeSpan.FromSeconds(30));

                var cacheData = await _cacheService.GetAsync<IEnumerable<Product>>(key);
                if (cacheData != null)
                    return cacheData;

                var result = await _repository.GetAllProductAsync(search, page, pageSize);
                await _cacheService.SetAsync(key, result, TimeSpan.FromMinutes(2));

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching products with search='{search}': {ex.Message}");
                throw;
            }
        }

        public async Task<Product> GetByIdProductAsync(int id)
        {
            try
            {
                return await _repository.GetByIdProductAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching Product with Id={id}: {ex.Message}");
                throw;
            }
        }

        public async Task AddProductAsync(Product product, IFormFile? file)
        {
            try
            {
                //CacheHelper.Remove(_cache, "Product");
                await _cacheService.RemoveByPrefixAsync("Product");
                await HandleFileUploadAsync(product, file);
                await _repository.AddProductAsync(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding Product '{product.Title}': {ex.Message}");
                throw;
            }
        }

        public async Task UpdateProductAsync(Product product, IFormFile? file)
        {
            try
            {
                //CacheHelper.Remove(_cache, "Product_");
                await _cacheService.RemoveByPrefixAsync("Product");
                await HandleFileUploadAsync(product, file);
                await _repository.UpdateProductAsync(product);
                await _auditService.AddAsync(new AuditLog
                {
                    EnityName = "Product",
                    EntityId = product.Id,
                    Action = "Update",
                    UserName = GetCurrentUser(),
                    TimeAction = DateTime.UtcNow
                });
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
                await _cacheService.RemoveByPrefixAsync("Product");
                await _repository.DeleteProductAsync(id);
                //CacheHelper.Remove(_cache, "Product_");
                await _auditService.AddAsync(new AuditLog
                {
                    EnityName = "Product",
                    EntityId = id,
                    Action = "Update",
                    UserName = GetCurrentUser(),
                    TimeAction = DateTime.UtcNow
                });
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
                return await _repository.GetCategoriesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching categories: {ex.Message}");
                throw;
            }
        }

        private async Task HandleFileUploadAsync(Product product, IFormFile? file)
        {
            if (file == null || file.Length == 0) return;

            try
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(_imageFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                product.ImageUrl = "/images/products/" + fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading file for Product '{product.Title}': {ex.Message}");
                throw;
            }
        }

        public async Task<int> GetTotalCountProductAsync(string search)
        {
            try
            {
                return await _repository.GetTotalProductCountAsync(search);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error counting products with search='{search}': {ex.Message}");
                throw;
            }
        }
    }
}
