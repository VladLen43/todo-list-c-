namespace TodoListApp.Core.Models;

public class Category
{
    public int CategoryId { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = "#808080";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDefault { get; set; } = false;

    // Navigation properties
    public virtual User? User { get; set; }
    public virtual ICollection<TodoTask> Tasks { get; set; } = new List<TodoTask>();
}
