using System.Windows;
using TodoListApp.WPF.Services;
using TodoListApp.WPF.ViewModels;
using TodoListApp.WPF.Views;

namespace TodoListApp.WPF;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var apiService = new ApiService();
        var loginViewModel = new LoginViewModel(apiService);
        var loginWindow = new LoginWindow(loginViewModel);
        
        loginWindow.Show();
    }
}
