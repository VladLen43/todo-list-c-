using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TodoListApp.Core.Enums;
using TodoListApp.WPF.Models;

namespace TodoListApp.WPF.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private string? _token;
    private const string BaseUrl = "http://localhost:5000/api";

    public ApiService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl)
        };
    }

    public string? GetToken() => _token;

    public bool IsAuthenticated() => !string.IsNullOrEmpty(_token);

    private void SetAuthHeader()
    {
        if (!string.IsNullOrEmpty(_token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/auth/login", new { username, password });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                _token = result?.Token;
                SetAuthHeader();
                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RegisterAsync(string username, string email, string password, string? firstName, string? lastName)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/auth/register", new
            {
                username,
                email,
                password,
                firstName,
                lastName
            });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<RegisterResponse>();
                _token = result?.Token;
                SetAuthHeader();
                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<TaskModel>> GetTasksAsync(TaskStatus? status = null, int? categoryId = null)
    {
        try
        {
            SetAuthHeader();
            var query = $"/tasks?";
            if (status.HasValue) query += $"status={status.Value}&";
            if (categoryId.HasValue) query += $"categoryId={categoryId.Value}&";

            var tasks = await _httpClient.GetFromJsonAsync<List<TaskModel>>(query);
            return tasks ?? new List<TaskModel>();
        }
        catch
        {
            return new List<TaskModel>();
        }
    }

    public async Task<TaskModel?> CreateTaskAsync(string title, string? description, TaskPriority priority, int? categoryId, DateTime? dueDate, List<string> tags)
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.PostAsJsonAsync("/tasks", new
            {
                title,
                description,
                priority,
                categoryId,
                dueDate,
                tags
            });

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TaskModel>();
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<TaskModel?> UpdateTaskAsync(int taskId, string? title, string? description, TaskPriority? priority, TaskStatus? status, int? categoryId, DateTime? dueDate)
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.PutAsJsonAsync($"/tasks/{taskId}", new
            {
                title,
                description,
                priority,
                status,
                categoryId,
                dueDate
            });

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TaskModel>();
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> DeleteTaskAsync(int taskId)
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.DeleteAsync($"/tasks/{taskId}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<CategoryModel>> GetCategoriesAsync()
    {
        try
        {
            SetAuthHeader();
            var categories = await _httpClient.GetFromJsonAsync<List<CategoryModel>>("/categories");
            return categories ?? new List<CategoryModel>();
        }
        catch
        {
            return new List<CategoryModel>();
        }
    }

    public async Task<CategoryModel?> CreateCategoryAsync(string name, string color)
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.PostAsJsonAsync("/categories", new { name, color });

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CategoryModel>();
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<Dictionary<string, int>> GetStatisticsAsync()
    {
        try
        {
            SetAuthHeader();
            var stats = await _httpClient.GetFromJsonAsync<Dictionary<string, int>>("/tasks/statistics");
            return stats ?? new Dictionary<string, int>();
        }
        catch
        {
            return new Dictionary<string, int>();
        }
    }

    private class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
    }

    private class RegisterResponse
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
