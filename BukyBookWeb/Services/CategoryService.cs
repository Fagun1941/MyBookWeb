using BukyBookWeb.IRepository;
using BukyBookWeb.Models;
using BukyBookWeb.Repositories;

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
            var categories = _repository.GetAllCategory(search,page,pageSize);

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

        public int GetTotalCount(string search)
        {
            var categoryCount = _repository.GetTotalCategoriesCount(search);

            return categoryCount;
        }
    }
}
