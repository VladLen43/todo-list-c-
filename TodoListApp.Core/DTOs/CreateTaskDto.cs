using TodoListApp.Core.Enums;

namespace TodoListApp.Core.DTOs;

public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public int? CategoryId { get; set; }
    public DateTime? DueDate { get; set; }
    public List<string> Tags { get; set; } = new();
}
