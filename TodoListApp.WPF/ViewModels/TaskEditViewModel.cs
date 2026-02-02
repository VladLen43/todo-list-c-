using System.Collections.ObjectModel;
using System.Windows.Input;
using TodoListApp.Core.Enums;
using TodoListApp.WPF.Helpers;
using TodoListApp.WPF.Models;
using TodoListApp.WPF.Services;

namespace TodoListApp.WPF.ViewModels;

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

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public event EventHandler? SaveCompleted;
    public event EventHandler? Cancelled;

    private bool CanSave()
    {
        return !string.IsNullOrWhiteSpace(Title);
    }

    private async Task LoadCategories()
    {
        var categories = await _apiService.GetCategoriesAsync();
        Categories = new ObservableCollection<CategoryModel>(categories);
    }

    private async Task Save()
    {
        if (_existingTask != null)
        {
            // Update existing task
            await _apiService.UpdateTaskAsync(
                _existingTask.TaskId,
                Title,
                Description,
                Priority,
                null,
                CategoryId,
                DueDate
            );
        }
        else
        {
            // Create new task
            await _apiService.CreateTaskAsync(
                Title,
                Description,
                Priority,
                CategoryId,
                DueDate,
                new List<string>()
            );
        }

        SaveCompleted?.Invoke(this, EventArgs.Empty);
    }

    private void Cancel()
    {
        Cancelled?.Invoke(this, EventArgs.Empty);
    }
}
