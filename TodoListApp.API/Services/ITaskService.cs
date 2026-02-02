using TodoListApp.Core.DTOs;
using TodoListApp.Core.Enums;

namespace TodoListApp.API.Services;

public interface ITaskService
{
    Task<List<TaskDto>> GetTasksAsync(int userId, TodoListApp.Core.Enums.TaskStatus? status = null, int? categoryId = null);
    Task<TaskDto?> GetTaskByIdAsync(int taskId, int userId);
    Task<TaskDto?> CreateTaskAsync(CreateTaskDto createTaskDto, int userId);
    Task<TaskDto?> UpdateTaskAsync(int taskId, UpdateTaskDto updateTaskDto, int userId);
    Task<bool> DeleteTaskAsync(int taskId, int userId);
    Task<Dictionary<string, int>> GetStatisticsAsync(int userId);
}
