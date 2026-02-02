using TodoListApp.Core.Enums;

namespace TodoListApp.Core.DTOs;

public class UpdateTaskDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public TaskPriority? Priority { get; set; }
    public TaskStatus? Status { get; set; }
    public int? CategoryId { get; set; }
    public DateTime? DueDate { get; set; }
    public List<string>? Tags { get; set; }
}
