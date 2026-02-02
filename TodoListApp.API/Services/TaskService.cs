using Microsoft.EntityFrameworkCore;
using TodoListApp.API.Data;
using TodoListApp.Core.DTOs;
using TodoListApp.Core.Enums;
using TodoListApp.Core.Models;

namespace TodoListApp.API.Services;

public class TaskService : ITaskService
{
    private readonly TodoDbContext _context;

    public TaskService(TodoDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskDto>> GetTasksAsync(int userId, TaskStatus? status = null, int? categoryId = null)
    {
        var query = _context.Tasks
            .Include(t => t.Category)
            .Include(t => t.Tags)
            .Where(t => t.UserId == userId && !t.IsDeleted);

        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        if (categoryId.HasValue)
        {
            query = query.Where(t => t.CategoryId == categoryId.Value);
        }

        var tasks = await query.OrderByDescending(t => t.CreatedAt).ToListAsync();

        return tasks.Select(t => new TaskDto
        {
            TaskId = t.TaskId,
            Title = t.Title,
            Description = t.Description,
            Priority = t.Priority,
            Status = t.Status,
            CategoryId = t.CategoryId,
            CategoryName = t.Category?.Name,
            CreatedAt = t.CreatedAt,
            DueDate = t.DueDate,
            CompletedAt = t.CompletedAt,
            Tags = t.Tags.Select(tag => tag.Name).ToList()
        }).ToList();
    }

    public async Task<TaskDto?> GetTaskByIdAsync(int taskId, int userId)
    {
        var task = await _context.Tasks
            .Include(t => t.Category)
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.TaskId == taskId && t.UserId == userId && !t.IsDeleted);

        if (task == null)
        {
            return null;
        }

        return new TaskDto
        {
            TaskId = task.TaskId,
            Title = task.Title,
            Description = task.Description,
            Priority = task.Priority,
            Status = task.Status,
            CategoryId = task.CategoryId,
            CategoryName = task.Category?.Name,
            CreatedAt = task.CreatedAt,
            DueDate = task.DueDate,
            CompletedAt = task.CompletedAt,
            Tags = task.Tags.Select(tag => tag.Name).ToList()
        };
    }

    public async Task<TaskDto?> CreateTaskAsync(CreateTaskDto createTaskDto, int userId)
    {
        var task = new TodoTask
        {
            UserId = userId,
            Title = createTaskDto.Title,
            Description = createTaskDto.Description,
            Priority = createTaskDto.Priority,
            CategoryId = createTaskDto.CategoryId,
            DueDate = createTaskDto.DueDate,
            CreatedAt = DateTime.UtcNow,
            Status = TaskStatus.New
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        // Handle tags
        if (createTaskDto.Tags.Any())
        {
            foreach (var tagName in createTaskDto.Tags)
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName && t.UserId == userId);
                if (tag == null)
                {
                    tag = new Tag { Name = tagName, UserId = userId };
                    _context.Tags.Add(tag);
                    await _context.SaveChangesAsync();
                }
                task.Tags.Add(tag);
            }
            await _context.SaveChangesAsync();
        }

        return await GetTaskByIdAsync(task.TaskId, userId);
    }

    public async Task<TaskDto?> UpdateTaskAsync(int taskId, UpdateTaskDto updateTaskDto, int userId)
    {
        var task = await _context.Tasks
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.TaskId == taskId && t.UserId == userId && !t.IsDeleted);

        if (task == null)
        {
            return null;
        }

        if (updateTaskDto.Title != null)
            task.Title = updateTaskDto.Title;

        if (updateTaskDto.Description != null)
            task.Description = updateTaskDto.Description;

        if (updateTaskDto.Priority.HasValue)
            task.Priority = updateTaskDto.Priority.Value;

        if (updateTaskDto.Status.HasValue)
        {
            task.Status = updateTaskDto.Status.Value;
            if (updateTaskDto.Status.Value == TaskStatus.Completed && task.CompletedAt == null)
            {
                task.CompletedAt = DateTime.UtcNow;
            }
            else if (updateTaskDto.Status.Value != TaskStatus.Completed)
            {
                task.CompletedAt = null;
            }
        }

        if (updateTaskDto.CategoryId.HasValue)
            task.CategoryId = updateTaskDto.CategoryId.Value;

        if (updateTaskDto.DueDate.HasValue)
            task.DueDate = updateTaskDto.DueDate.Value;

        if (updateTaskDto.Tags != null)
        {
            task.Tags.Clear();
            foreach (var tagName in updateTaskDto.Tags)
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName && t.UserId == userId);
                if (tag == null)
                {
                    tag = new Tag { Name = tagName, UserId = userId };
                    _context.Tags.Add(tag);
                    await _context.SaveChangesAsync();
                }
                task.Tags.Add(tag);
            }
        }

        task.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return await GetTaskByIdAsync(taskId, userId);
    }

    public async Task<bool> DeleteTaskAsync(int taskId, int userId)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.TaskId == taskId && t.UserId == userId);

        if (task == null)
        {
            return false;
        }

        task.IsDeleted = true;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Dictionary<string, int>> GetStatisticsAsync(int userId)
    {
        var tasks = await _context.Tasks
            .Where(t => t.UserId == userId && !t.IsDeleted)
            .ToListAsync();

        return new Dictionary<string, int>
        {
            { "Total", tasks.Count },
            { "New", tasks.Count(t => t.Status == TaskStatus.New) },
            { "InProgress", tasks.Count(t => t.Status == TaskStatus.InProgress) },
            { "Completed", tasks.Count(t => t.Status == TaskStatus.Completed) },
            { "Overdue", tasks.Count(t => t.DueDate.HasValue && t.DueDate.Value < DateTime.UtcNow && t.Status != TaskStatus.Completed) }
        };
    }
}
