using TodoListApp.Core.DTOs;
using TodoListApp.Core.Models;

namespace TodoListApp.API.Services;

public interface IAuthService
{
    Task<User?> RegisterAsync(RegisterDto registerDto);
    Task<string?> LoginAsync(LoginDto loginDto);
    Task<User?> GetUserByIdAsync(int userId);
    string GenerateJwtToken(User user);
}
