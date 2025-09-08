using BukyBookWeb.Models;

namespace BukyBookWeb.IRepository
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategory(string search, int page,int pageSize);
        Category? GetByIdCategory(int id);
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int id);
        int GetTotalCategoriesCount(string search);
    }
}
