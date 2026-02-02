using Microsoft.AspNetCore.Mvc;
using TodoListApp.API.Services;
using TodoListApp.Core.DTOs;

namespace TodoListApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _authService.RegisterAsync(registerDto);

        if (user == null)
        {
            return BadRequest(new { message = "Пользователь с таким именем или email уже существует" });
        }

        var token = _authService.GenerateJwtToken(user);

        return Ok(new
        {
            userId = user.UserId,
            username = user.Username,
            email = user.Email,
            token = token
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var token = await _authService.LoginAsync(loginDto);

        if (token == null)
        {
            return Unauthorized(new { message = "Неверное имя пользователя или пароль" });
        }

        return Ok(new { token = token });
    }
}
