using BukyBookWeb.Models;
using System.Collections.Generic;

namespace BukyBookWeb.Services
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategory(string search, int page, int pageSize);
        Category GetByIdCategory(int id);
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int id);
        int GetTotalCount(string search);
    }
}
