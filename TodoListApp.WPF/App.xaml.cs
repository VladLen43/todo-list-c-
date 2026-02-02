using System.Windows;
using TodoListApp.WPF.Services;
using TodoListApp.WPF.ViewModels;
using TodoListApp.WPF.Views;

namespace TodoListApp.WPF;

public partial class App : Application
{
    public static IApiService ApiService { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ApiService = new ApiService();
        var loginViewModel = new LoginViewModel(ApiService);
        var loginWindow = new LoginWindow(loginViewModel);
        
        loginWindow.Show();
    }
}
