using System.Windows;
using TodoListApp.WPF.Services;
using TodoListApp.WPF.ViewModels;

namespace TodoListApp.WPF.Views;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;

    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new MainViewModel(App.ApiService);
        DataContext = _viewModel;

        _viewModel.TaskCreateRequested += (s, e) =>
        {
            var editWindow = new TaskEditWindow(null);
            editWindow.ShowDialog();
            if (editWindow.DialogResult == true)
            {
                _viewModel.RefreshCommand.Execute(null);
            }
        };

        _viewModel.TaskEditRequested += (s, task) =>
        {
            var editWindow = new TaskEditWindow(task);
            editWindow.ShowDialog();
            if (editWindow.DialogResult == true)
            {
                _viewModel.RefreshCommand.Execute(null);
            }
        };
    }
}
