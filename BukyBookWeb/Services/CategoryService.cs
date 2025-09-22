using BukyBookWeb.Helpers;
using BukyBookWeb.IRepository;
using BukyBookWeb.Models;
using BukyBookWeb.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BukyBookWeb.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMemoryCache _cache;

        public CategoryService(ICategoryRepository repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache; 
        }

        public IEnumerable<Category> GetAllCategory(string? search, int page, int pageSize)
        {
            try
            {
                string key = $"Category_{search}_{page}_{pageSize}";
                return CacheHelper.GetOrSet(_cache, key,
                    () => _repository.GetAllCategory(search!, page, pageSize),
                    absolute: TimeSpan.FromMinutes(2),
                    sliding: TimeSpan.FromSeconds(30));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching categories with search='{search}': {ex.Message}");
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
                Console.WriteLine($"Error fetching Category with Id={id}: {ex.Message}");
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
                CacheHelper.Remove(_cache, $"Category_");
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
                CacheHelper.Remove(_cache, $"Category_");
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
                CacheHelper.Remove(_cache, $"Category_");
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
                Console.WriteLine($"Error counting categories with search='{search}': {ex.Message}");
                throw;
            }
        }
    }
}
