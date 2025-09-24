using BukyBookWeb.Models;

namespace BukyBookWeb.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoryAsync(string? search, int page, int pageSize);
        Task<int> GetTotalCountAsync(string? search);
        Task<Category?> GetByIdCategoryAsync(int id);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
    }
}
