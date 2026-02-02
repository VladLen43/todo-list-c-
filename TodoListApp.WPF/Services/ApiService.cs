using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
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
        _httpClient = new HttpClient();
    }
    
    private string GetFullUrl(string endpoint)
    {
        // Убираем начальный / если есть
        if (endpoint.StartsWith("/"))
            endpoint = endpoint.Substring(1);
        return $"{BaseUrl}/{endpoint}";
    }

    public string? GetToken() => _token;

    public bool IsAuthenticated() => !string.IsNullOrEmpty(_token);

    private void SetAuthHeader()
    {
        // Удаляем старый заголовок, если есть
        _httpClient.DefaultRequestHeaders.Remove("Authorization");
        
        // Устанавливаем новый заголовок, если токен есть
        if (!string.IsNullOrEmpty(_token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(GetFullUrl("auth/login"), new { username, password });

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

    public async Task<string?> RegisterAsync(string username, string email, string password, string? firstName, string? lastName)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(GetFullUrl("auth/register"), new
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
                return null; // Успех - нет ошибки
            }

            // Пытаемся прочитать сообщение об ошибке из ответа
            try
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                var errorMessage = errorResponse?.Message ?? "Ошибка регистрации";
                
                // Если 404, добавляем информацию о URL
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    var requestUrl = response.RequestMessage?.RequestUri?.ToString() ?? "/auth/register";
                    return $"Эндпоинт не найден (404). Проверьте, что API запущен и доступен по адресу {BaseUrl}. Запрос: {requestUrl}";
                }
                
                return errorMessage;
            }
            catch
            {
                var statusCode = response.StatusCode;
                var requestUrl = response.RequestMessage?.RequestUri?.ToString() ?? "/auth/register";
                
                if (statusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return $"Эндпоинт не найден (404). Проверьте, что API запущен на {BaseUrl}. Запрос: {requestUrl}";
                }
                
                return $"Ошибка регистрации: {statusCode}. URL: {requestUrl}";
            }
        }
        catch (HttpRequestException ex)
        {
            return $"Не удалось подключиться к серверу. Убедитесь, что API запущен. Ошибка: {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"Ошибка: {ex.Message}";
        }
    }

    public async Task<List<TaskModel>> GetTasksAsync(TodoListApp.Core.Enums.TaskStatus? status = null, int? categoryId = null)
    {
        try
        {
            SetAuthHeader();
            var query = "tasks?";
            if (status.HasValue) query += $"status={status.Value}&";
            if (categoryId.HasValue) query += $"categoryId={categoryId.Value}&";

            var tasks = await _httpClient.GetFromJsonAsync<List<TaskModel>>(GetFullUrl(query));
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
            var response = await _httpClient.PostAsJsonAsync(GetFullUrl("tasks"), new
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

            // Читаем сообщение об ошибке
            try
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                throw new Exception(errorResponse?.Message ?? $"Ошибка создания задачи: {response.StatusCode}");
            }
            catch (JsonException)
            {
                var errorText = await response.Content.ReadAsStringAsync();
                throw new Exception($"Ошибка создания задачи: {response.StatusCode}. {errorText}");
            }
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Не удалось подключиться к серверу: {ex.Message}");
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<TaskModel?> UpdateTaskAsync(int taskId, string? title, string? description, TaskPriority? priority, TodoListApp.Core.Enums.TaskStatus? status, int? categoryId, DateTime? dueDate)
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.PutAsJsonAsync(GetFullUrl($"tasks/{taskId}"), new
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

            // Читаем сообщение об ошибке
            try
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                throw new Exception(errorResponse?.Message ?? $"Ошибка обновления задачи: {response.StatusCode}");
            }
            catch (JsonException)
            {
                var errorText = await response.Content.ReadAsStringAsync();
                throw new Exception($"Ошибка обновления задачи: {response.StatusCode}. {errorText}");
            }
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Не удалось подключиться к серверу: {ex.Message}");
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<bool> DeleteTaskAsync(int taskId)
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.DeleteAsync(GetFullUrl($"tasks/{taskId}"));
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
            var categories = await _httpClient.GetFromJsonAsync<List<CategoryModel>>(GetFullUrl("categories"));
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
            var response = await _httpClient.PostAsJsonAsync(GetFullUrl("categories"), new { name, color });

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
            var stats = await _httpClient.GetFromJsonAsync<Dictionary<string, int>>(GetFullUrl("tasks/statistics"));
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

    private class ErrorResponse
    {
        public string? Message { get; set; }
    }
}
