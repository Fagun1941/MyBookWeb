using BukyBookWeb.IRepository;
using BukyBookWeb.Models;
using BukyBookWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BukyBookWeb.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Category> GetAllCategory(string search, int page, int pageSize)
        {
            try
            {
                var categories = _repository.GetAllCategory(search, page, pageSize);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    categories = categories
                        .Where(c => c.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
                }

                return categories.OrderBy(c => int.TryParse(c.DisplayOrder, out var num) ? num : int.MaxValue);
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
