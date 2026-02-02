using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListApp.Core.Enums;
using TodoListApp.WPF.Models;

namespace TodoListApp.WPF.Services;

public interface IApiService
{
    Task<bool> LoginAsync(string username, string password);
    Task<bool> RegisterAsync(string username, string email, string password, string? firstName, string? lastName);
    Task<List<TaskModel>> GetTasksAsync(TaskStatus? status = null, int? categoryId = null);
    Task<TaskModel?> CreateTaskAsync(string title, string? description, TaskPriority priority, int? categoryId, System.DateTime? dueDate, List<string> tags);
    Task<TaskModel?> UpdateTaskAsync(int taskId, string? title, string? description, TaskPriority? priority, TaskStatus? status, int? categoryId, System.DateTime? dueDate);
    Task<bool> DeleteTaskAsync(int taskId);
    Task<List<CategoryModel>> GetCategoriesAsync();
    Task<CategoryModel?> CreateCategoryAsync(string name, string color);
    Task<Dictionary<string, int>> GetStatisticsAsync();
    string? GetToken();
    bool IsAuthenticated();
}
