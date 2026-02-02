using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListApp.Core.Enums;
using TodoListApp.WPF.Models;

namespace TodoListApp.WPF.Services;

public interface IApiService
{
    Task<bool> LoginAsync(string username, string password);
    Task<string?> RegisterAsync(string username, string email, string password, string? firstName, string? lastName);
    Task<List<TaskModel>> GetTasksAsync(TodoListApp.Core.Enums.TaskStatus? status = null, int? categoryId = null);
    Task<TaskModel?> CreateTaskAsync(string title, string? description, TaskPriority priority, int? categoryId, System.DateTime? dueDate, List<string> tags);
    Task<TaskModel?> UpdateTaskAsync(int taskId, string? title, string? description, TaskPriority? priority, TodoListApp.Core.Enums.TaskStatus? status, int? categoryId, System.DateTime? dueDate);
    Task<bool> DeleteTaskAsync(int taskId);
    Task<List<CategoryModel>> GetCategoriesAsync();
    Task<CategoryModel?> CreateCategoryAsync(string name, string color);
    Task<Dictionary<string, int>> GetStatisticsAsync();
    string? GetToken();
    bool IsAuthenticated();
}
