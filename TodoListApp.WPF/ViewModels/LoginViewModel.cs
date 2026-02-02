using System.Windows;
using System.Windows.Input;
using TodoListApp.WPF.Helpers;
using TodoListApp.WPF.Services;

namespace TodoListApp.WPF.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly IApiService _apiService;
    private string _username = string.Empty;
    private string _password = string.Empty;
    private string _errorMessage = string.Empty;

    public LoginViewModel(IApiService apiService)
    {
        _apiService = apiService;
        LoginCommand = new RelayCommand(async _ => await Login(), _ => CanLogin());
        RegisterCommand = new RelayCommand(_ => ShowRegister());
    }

    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public ICommand LoginCommand { get; }
    public ICommand RegisterCommand { get; }

    public event EventHandler? LoginSuccessful;
    public event EventHandler? RegisterRequested;

    private bool CanLogin()
    {
        return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
    }

    private async Task Login()
    {
        ErrorMessage = string.Empty;
        var success = await _apiService.LoginAsync(Username, Password);

        if (success)
        {
            LoginSuccessful?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            ErrorMessage = "Неверное имя пользователя или пароль";
        }
    }

    private void ShowRegister()
    {
        RegisterRequested?.Invoke(this, EventArgs.Empty);
    }
}
