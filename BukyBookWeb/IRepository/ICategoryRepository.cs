using BukyBookWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BukyBookWeb.IRepository
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoryAsync(string search, int page, int pageSize);
        Task<Category?> GetByIdCategoryAsync(int id);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
        Task<int> GetTotalCategoriesCountAsync(string search);
    }
}
