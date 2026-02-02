using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoListApp.API.Services;

namespace TodoListApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userIdClaim ?? "0");
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var userId = GetUserId();
        var categories = await _categoryService.GetCategoriesAsync(userId);
        return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetUserId();
        var category = await _categoryService.CreateCategoryAsync(request.Name, request.Color, userId);

        if (category == null)
        {
            return BadRequest(new { message = "Не удалось создать категорию" });
        }

        return Ok(category);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var userId = GetUserId();
        var result = await _categoryService.DeleteCategoryAsync(id, userId);

        if (!result)
        {
            return NotFound(new { message = "Категория не найдена или является системной" });
        }

        return NoContent();
    }
}

public class CreateCategoryRequest
{
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = "#808080";
}
