using System.Windows;
using TodoListApp.WPF.Models;
using TodoListApp.WPF.Services;
using TodoListApp.WPF.ViewModels;

namespace TodoListApp.WPF.Views;

public partial class TaskEditWindow : Window
{
    private readonly TaskEditViewModel _viewModel;

    public TaskEditWindow(TaskModel? existingTask)
    {
        InitializeComponent();
        _viewModel = new TaskEditViewModel(new ApiService(), existingTask);
        DataContext = _viewModel;

        _viewModel.SaveCompleted += (s, e) =>
        {
            DialogResult = true;
            Close();
        };

        _viewModel.Cancelled += (s, e) =>
        {
            DialogResult = false;
            Close();
        };
    }
}
