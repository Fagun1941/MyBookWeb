


////***************************    this is my using IMemory Caches   ******************************///////////////////////////////////


//using BukyBookWeb.IRepository;
//using BukyBookWeb.Models;
//using System;
//using System.Collections.Generic;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace BukyBookWeb.Services
//{
//    public class CategoryService : ICategoryService
//    {
//        private readonly ICategoryRepository _repository;

//        // private readonly IMemoryCache _cache; 
//        // private readonly IDatabase _redis; // ❌ direct Redis DB removed
//        private readonly ICacheService _cacheService; // ✅ Use cache service wrapper

//        public CategoryService(ICategoryRepository repository, /*IMemoryCache cache, IConnectionMultiplexer redis*/ ICacheService cacheService)
//        {
//            _repository = repository;

//            // _cache = cache;  
//            // _redis = redis.GetDatabase(); 
//            _cacheService = cacheService;
//        }

//        public IEnumerable<Category> GetAllCategory(string? search, int page, int pageSize)
//        {
//            try
//            {
//                string key = $"Category_{search}_{page}_{pageSize}";

//                // --- Old direct Redis ---
//                // var cachedData = _redis.StringGet(key);
//                // if (cachedData.HasValue)
//                // {
//                //     return JsonSerializer.Deserialize<IEnumerable<Category>>(cachedData)!;
//                // }

//                // --- New via CacheService ---
//                var cachedData = _cacheService.GetAsync<IEnumerable<Category>>(key).Result;
//                if (cachedData != null)
//                {
//                    return cachedData;
//                }

//                // Fallback to DB if not in cache
//                var result = _repository.GetAllCategory(search!, page, pageSize);

//                // --- Old direct Redis ---
//                // _redis.StringSet(key, JsonSerializer.Serialize(result), TimeSpan.FromMinutes(2));

//                // --- New via CacheService ---
//                _cacheService.SetAsync(key, result, TimeSpan.FromMinutes(2)).Wait();

//                return result;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error fetching categories with search='{search}': {ex.Message}");
//                throw;
//            }
//        }

//        public Category GetByIdCategory(int id)
//        {
//            try
//            {
//                return _repository.GetByIdCategory(id);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error fetching Category with Id={id}: {ex.Message}");
//                throw;
//            }
//        }

//        public void AddCategory(Category category)
//        {
//            try
//            {
//                if (string.IsNullOrWhiteSpace(category.Name))
//                    throw new ArgumentException("Category name cannot be empty.");

//                _repository.AddCategory(category);

//                // --- Old ---
//                // CacheHelper.Remove(_cache, $"Category_"); 
//                // _redis.KeyDelete("Category_*"); 

//                // --- New ---
//                _cacheService.RemoveByPrefixAsync("Category_").Wait();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error adding Category '{category.Name}': {ex.Message}");
//                throw;
//            }
//        }

//        public void UpdateCategory(Category category)
//        {
//            try
//            {
//                _repository.UpdateCategory(category);

//                // --- Old ---
//                // CacheHelper.Remove(_cache, $"Category_"); 
//                // _redis.KeyDelete("Category_*"); 

//                // --- New ---
//                _cacheService.RemoveByPrefixAsync("Category_").Wait();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error updating Category Id={category.Id}: {ex.Message}");
//                throw;
//            }
//        }

//        public void DeleteCategory(int id)
//        {
//            try
//            {
//                _repository.DeleteCategory(id);

//                // --- Old ---
//                // CacheHelper.Remove(_cache, $"Category_"); 
//                // _redis.KeyDelete("Category_*"); 

//                // --- New ---
//                _cacheService.RemoveByPrefixAsync("Category_").Wait();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error deleting Category Id={id}: {ex.Message}");
//                throw;
//            }
//        }

//        public int GetTotalCount(string search)
//        {
//            try
//            {
//                return _repository.GetTotalCategoriesCount(search);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error counting categories with search='{search}': {ex.Message}");
//                throw;
//            }
//        }
//    }
//}


////***************************    this is my using Redis Caches   ******************************///////////////////////////////////


using BukyBookWeb.IRepository;
using BukyBookWeb.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BukyBookWeb.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly ICacheService _cacheService;

       
        public CategoryService(ICategoryRepository repository, ICacheService cacheService)
        {
            _repository = repository ;
            _cacheService = cacheService ;
        }

        public IEnumerable<Category> GetAllCategory(string? search, int page, int pageSize)
        {
            try
            {
                string key = $"Category_{search}_{page}_{pageSize}";

                var cachedData = _cacheService.GetAsync<IEnumerable<Category>>(key).Result;
                if (cachedData != null)
                    return cachedData;

                var result = _repository.GetAllCategory(search!, page, pageSize);
                _cacheService.SetAsync(key, result, TimeSpan.FromMinutes(2)).Wait();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching categories: {ex.Message}");
                throw;
            }
        }

        public Category GetByIdCategory(int id)
        {
            try
            {
                return _repository.GetByIdCategory(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching Category Id={id}: {ex.Message}");
                throw;
            }
        }

        public void AddCategory(Category category)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(category.Name))
                    throw new ArgumentException("Category name cannot be empty.");

                _repository.AddCategory(category);
                _cacheService.RemoveByPrefixAsync("Category").Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding Category '{category.Name}': {ex.Message}");
                throw;
            }
        }

        public void UpdateCategory(Category category)
        {
            try
            {
                _repository.UpdateCategory(category);
                _cacheService.RemoveByPrefixAsync("Category").Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Category Id={category.Id}: {ex.Message}");
                throw;
            }
        }

        public void DeleteCategory(int id)
        {
            try
            {
                _repository.DeleteCategory(id);
                _cacheService.RemoveByPrefixAsync("Category").Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting Category Id={id}: {ex.Message}");
                throw;
            }
        }

        public int GetTotalCount(string search)
        {
            try
            {
                return _repository.GetTotalCategoriesCount(search);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error counting categories: {ex.Message}");
                throw;
            }
        }
    }
}
