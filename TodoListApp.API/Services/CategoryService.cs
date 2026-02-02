using Microsoft.EntityFrameworkCore;
using TodoListApp.API.Data;
using TodoListApp.Core.DTOs;
using TodoListApp.Core.Models;

namespace TodoListApp.API.Services;

public class CategoryService : ICategoryService
{
    private readonly TodoDbContext _context;

    public CategoryService(TodoDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoryDto>> GetCategoriesAsync(int userId)
    {
        var categories = await _context.Categories
            .Include(c => c.Tasks)
            .Where(c => c.UserId == userId)
            .ToListAsync();

        return categories.Select(c => new CategoryDto
        {
            CategoryId = c.CategoryId,
            Name = c.Name,
            Color = c.Color,
            TaskCount = c.Tasks.Count(t => !t.IsDeleted)
        }).ToList();
    }

    public async Task<CategoryDto?> CreateCategoryAsync(string name, string color, int userId)
    {
        var category = new Category
        {
            UserId = userId,
            Name = name,
            Color = color,
            CreatedAt = DateTime.UtcNow
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return new CategoryDto
        {
            CategoryId = category.CategoryId,
            Name = category.Name,
            Color = category.Color,
            TaskCount = 0
        };
    }

    public async Task<bool> DeleteCategoryAsync(int categoryId, int userId)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.CategoryId == categoryId && c.UserId == userId);

        if (category == null || category.IsDefault)
        {
            return false;
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return true;
    }
}
