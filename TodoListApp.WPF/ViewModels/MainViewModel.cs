using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoListApp.Core.Enums;
using TodoListApp.WPF.Helpers;
using TodoListApp.WPF.Models;
using TodoListApp.WPF.Services;

namespace TodoListApp.WPF.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IApiService _apiService;
    private ObservableCollection<TaskModel> _tasks = new();
    private ObservableCollection<CategoryModel> _categories = new();
    private TaskModel? _selectedTask;
    private string _searchText = string.Empty;
    private TodoListApp.Core.Enums.TaskStatus? _filterStatus;
    private int? _filterCategoryId;
    private Dictionary<string, int> _statistics = new();

    public MainViewModel(IApiService apiService)
    {
        _apiService = apiService;
        
        CreateTaskCommand = new RelayCommand(_ => CreateTask());
        EditTaskCommand = new RelayCommand(_ => EditTask(), _ => SelectedTask != null);
        DeleteTaskCommand = new RelayCommand(async _ => await DeleteTask(), _ => SelectedTask != null);
        RefreshCommand = new RelayCommand(async _ => await LoadData());
        CompleteTaskCommand = new RelayCommand(async _ => await CompleteTask(), _ => SelectedTask != null && SelectedTask.Status != TodoListApp.Core.Enums.TaskStatus.Completed);
        
        _ = LoadData();
    }

    public ObservableCollection<TaskModel> Tasks
    {
        get => _tasks;
        set => SetProperty(ref _tasks, value);
    }

    public ObservableCollection<CategoryModel> Categories
    {
        get => _categories;
        set => SetProperty(ref _categories, value);
    }

    public TaskModel? SelectedTask
    {
        get => _selectedTask;
        set => SetProperty(ref _selectedTask, value);
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            _ = LoadTasks();
        }
    }

    public Dictionary<string, int> Statistics
    {
        get => _statistics;
        set => SetProperty(ref _statistics, value);
    }

    public ICommand CreateTaskCommand { get; }
    public ICommand EditTaskCommand { get; }
    public ICommand DeleteTaskCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand CompleteTaskCommand { get; }

    public event EventHandler<TaskModel>? TaskEditRequested;
    public event EventHandler? TaskCreateRequested;

    private async Task LoadData()
    {
        await LoadTasks();
        await LoadCategories();
        await LoadStatistics();
    }

    private async Task LoadTasks()
    {
        var tasks = await _apiService.GetTasksAsync(_filterStatus, _filterCategoryId);
        
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            tasks = tasks.Where(t => 
                t.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                (t.Description?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)
            ).ToList();
        }

        Tasks = new ObservableCollection<TaskModel>(tasks);
    }

    private async Task LoadCategories()
    {
        var categories = await _apiService.GetCategoriesAsync();
        Categories = new ObservableCollection<CategoryModel>(categories);
    }

    private async Task LoadStatistics()
    {
        Statistics = await _apiService.GetStatisticsAsync();
    }

    private void CreateTask()
    {
        TaskCreateRequested?.Invoke(this, EventArgs.Empty);
    }

    private void EditTask()
    {
        if (SelectedTask != null)
        {
            TaskEditRequested?.Invoke(this, SelectedTask);
        }
    }

    private async Task DeleteTask()
    {
        if (SelectedTask != null)
        {
            var success = await _apiService.DeleteTaskAsync(SelectedTask.TaskId);
            if (success)
            {
                await LoadData();
            }
        }
    }

    private async Task CompleteTask()
    {
        if (SelectedTask != null)
        {
            await _apiService.UpdateTaskAsync(
                SelectedTask.TaskId,
                null, null, null,
                TodoListApp.Core.Enums.TaskStatus.Completed,
                null, null
            );
            await LoadData();
        }
    }

    public async Task SetFilter(TodoListApp.Core.Enums.TaskStatus? status, int? categoryId)
    {
        _filterStatus = status;
        _filterCategoryId = categoryId;
        await LoadTasks();
    }
}
