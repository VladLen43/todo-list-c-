using TodoListApp.Core.Enums;

namespace TodoListApp.Core.Models;

public class TodoTask
{
    public int TaskId { get; set; }
    public int UserId { get; set; }
    public int? CategoryId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public TaskStatus Status { get; set; } = TaskStatus.New;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool IsDeleted { get; set; } = false;

    // Navigation properties
    public virtual User? User { get; set; }
    public virtual Category? Category { get; set; }
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
