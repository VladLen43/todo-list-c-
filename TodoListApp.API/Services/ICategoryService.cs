using TodoListApp.Core.DTOs;

namespace TodoListApp.API.Services;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetCategoriesAsync(int userId);
    Task<CategoryDto?> CreateCategoryAsync(string name, string color, int userId);
    Task<bool> DeleteCategoryAsync(int categoryId, int userId);
}
