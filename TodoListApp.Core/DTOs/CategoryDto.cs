namespace TodoListApp.Core.DTOs;

public class CategoryDto
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = "#808080";
    public int TaskCount { get; set; }
}
