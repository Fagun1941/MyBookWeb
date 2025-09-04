using BukyBookWeb.Models;
using BukyBookWeb.Repositories;

namespace BukyBookWeb.Services
{
    public class CategoryService
    {
        private readonly CategoryRepository _repository;

        public CategoryService(CategoryRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Category> GetAllCategory(string search)
        {
            var categories = _repository.GetAllCategory();

            if (!string.IsNullOrWhiteSpace(search))
            {
                categories = categories
                    .Where(c => c.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            return categories.OrderBy(c => int.TryParse(c.DisplayOrder, out var num) ? num : int.MaxValue);
        }


        public Category GetByIdCategory(int id)
        {
            return _repository.GetByIdCategory(id);
        }

        public void AddCategory(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
                throw new ArgumentException("Category name cannot be empty.");

            _repository.AddCategory(category);
        }

        public void UpdateCategory(Category category)
        {
            _repository.UpdateCategory(category);
        }

        public void DeleteCategory(int id)
        {
            _repository.DeleteCategory(id);
        }
    }
}
