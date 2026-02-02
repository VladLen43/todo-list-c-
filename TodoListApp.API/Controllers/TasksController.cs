using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoListApp.API.Services;
using TodoListApp.Core.DTOs;
using TodoListApp.Core.Enums;

namespace TodoListApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userIdClaim ?? "0");
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks([FromQuery] TodoListApp.Core.Enums.TaskStatus? status = null, [FromQuery] int? categoryId = null)
    {
        var userId = GetUserId();
        var tasks = await _taskService.GetTasksAsync(userId, status, categoryId);
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id)
    {
        var userId = GetUserId();
        var task = await _taskService.GetTaskByIdAsync(id, userId);

        if (task == null)
        {
            return NotFound(new { message = "Задача не найдена" });
        }

        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto createTaskDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetUserId();
        var task = await _taskService.CreateTaskAsync(createTaskDto, userId);

        if (task == null)
        {
            return BadRequest(new { message = "Не удалось создать задачу" });
        }

        return CreatedAtAction(nameof(GetTask), new { id = task.TaskId }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto updateTaskDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetUserId();
        var task = await _taskService.UpdateTaskAsync(id, updateTaskDto, userId);

        if (task == null)
        {
            return NotFound(new { message = "Задача не найдена" });
        }

        return Ok(task);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var userId = GetUserId();
        var result = await _taskService.DeleteTaskAsync(id, userId);

        if (!result)
        {
            return NotFound(new { message = "Задача не найдена" });
        }

        return NoContent();
    }

    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatistics()
    {
        var userId = GetUserId();
        var statistics = await _taskService.GetStatisticsAsync(userId);
        return Ok(statistics);
    }
}
