using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoListApp.Core.Enums;
using TodoListApp.WPF.Helpers;
using TodoListApp.WPF.Models;
using TodoListApp.WPF.Services;

namespace TodoListApp.WPF.ViewModels;

public class PriorityItem
{
    public TaskPriority Value { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class TaskEditViewModel : ViewModelBase
{
    private readonly IApiService _apiService;
    private readonly TaskModel? _existingTask;
    private string _title = string.Empty;
    private string _description = string.Empty;
    private TaskPriority _priority = TaskPriority.Medium;
    private int? _categoryId;
    private DateTime? _dueDate;
    private ObservableCollection<CategoryModel> _categories = new();
    private ObservableCollection<PriorityItem> _priorities = new();

    public TaskEditViewModel(IApiService apiService, TaskModel? existingTask = null)
    {
        _apiService = apiService;
        _existingTask = existingTask;

        if (_existingTask != null)
        {
            Title = _existingTask.Title;
            Description = _existingTask.Description ?? string.Empty;
            Priority = _existingTask.Priority;
            CategoryId = _existingTask.CategoryId;
            DueDate = _existingTask.DueDate;
        }

        SaveCommand = new RelayCommand(async _ => await Save(), _ => CanSave());
        CancelCommand = new RelayCommand(_ => Cancel());

        LoadPriorities();
        _ = LoadCategories();
    }

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public TaskPriority Priority
    {
        get => _priority;
        set => SetProperty(ref _priority, value);
    }

    public int? CategoryId
    {
        get => _categoryId;
        set => SetProperty(ref _categoryId, value);
    }

    public DateTime? DueDate
    {
        get => _dueDate;
        set => SetProperty(ref _dueDate, value);
    }

    public ObservableCollection<CategoryModel> Categories
    {
        get => _categories;
        set => SetProperty(ref _categories, value);
    }

    public ObservableCollection<PriorityItem> Priorities
    {
        get => _priorities;
        set => SetProperty(ref _priorities, value);
    }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public event EventHandler? SaveCompleted;
    public event EventHandler? Cancelled;

    private bool CanSave()
    {
        return !string.IsNullOrWhiteSpace(Title);
    }

    private void LoadPriorities()
    {
        Priorities = new ObservableCollection<PriorityItem>
        {
            new PriorityItem { Value = TaskPriority.Low, Name = "Низкий" },
            new PriorityItem { Value = TaskPriority.Medium, Name = "Средний" },
            new PriorityItem { Value = TaskPriority.High, Name = "Высокий" },
            new PriorityItem { Value = TaskPriority.Critical, Name = "Критический" }
        };
    }

    private async Task LoadCategories()
    {
        var categories = await _apiService.GetCategoriesAsync();
        Categories = new ObservableCollection<CategoryModel>(categories);
    }

    private async Task Save()
    {
        try
        {
            if (_existingTask != null)
            {
                // Update existing task
                var result = await _apiService.UpdateTaskAsync(
                    _existingTask.TaskId,
                    Title,
                    Description,
                    Priority,
                    null,
                    CategoryId,
                    DueDate
                );
                
                if (result == null)
                {
                    System.Windows.MessageBox.Show("Не удалось обновить задачу. Проверьте подключение к серверу.", "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                // Create new task
                var result = await _apiService.CreateTaskAsync(
                    Title,
                    Description,
                    Priority,
                    CategoryId,
                    DueDate,
                    new List<string>()
                );
                
                if (result == null)
                {
                    System.Windows.MessageBox.Show("Не удалось создать задачу. Проверьте подключение к серверу.", "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    return;
                }
            }

            SaveCompleted?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"Ошибка при сохранении задачи: {ex.Message}", "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
    }

    private void Cancel()
    {
        Cancelled?.Invoke(this, EventArgs.Empty);
    }
}
