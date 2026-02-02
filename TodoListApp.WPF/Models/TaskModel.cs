using System;
using System.Collections.Generic;
using TodoListApp.Core.Enums;

namespace TodoListApp.WPF.Models;

public class TaskModel
{
    public int TaskId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; }
    public TaskStatus Status { get; set; }
    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public List<string> Tags { get; set; } = new();
}
