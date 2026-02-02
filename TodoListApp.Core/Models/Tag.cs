namespace TodoListApp.Core.Models;

public class Tag
{
    public int TagId { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual User? User { get; set; }
    public virtual ICollection<TodoTask> Tasks { get; set; } = new List<TodoTask>();
}
