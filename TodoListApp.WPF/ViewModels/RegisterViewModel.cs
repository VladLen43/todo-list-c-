using System.Windows.Input;
using TodoListApp.WPF.Helpers;
using TodoListApp.WPF.Services;

namespace TodoListApp.WPF.ViewModels;

public class RegisterViewModel : ViewModelBase
{
    private readonly IApiService _apiService;
    private string _username = string.Empty;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string _confirmPassword = string.Empty;
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private string _errorMessage = string.Empty;

    public RegisterViewModel(IApiService apiService)
    {
        _apiService = apiService;
        RegisterCommand = new RelayCommand(async _ => await Register(), _ => CanRegister());
        BackCommand = new RelayCommand(_ => GoBack());
    }

    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string ConfirmPassword
    {
        get => _confirmPassword;
        set => SetProperty(ref _confirmPassword, value);
    }

    public string FirstName
    {
        get => _firstName;
        set => SetProperty(ref _firstName, value);
    }

    public string LastName
    {
        get => _lastName;
        set => SetProperty(ref _lastName, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public ICommand RegisterCommand { get; }
    public ICommand BackCommand { get; }

    public event EventHandler? RegisterSuccessful;
    public event EventHandler? BackRequested;

    private bool CanRegister()
    {
        return !string.IsNullOrWhiteSpace(Username) &&
               !string.IsNullOrWhiteSpace(Email) &&
               !string.IsNullOrWhiteSpace(Password) &&
               Password == ConfirmPassword;
    }

    private async Task Register()
    {
        ErrorMessage = string.Empty;

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Пароли не совпадают";
            return;
        }

        var success = await _apiService.RegisterAsync(Username, Email, Password, FirstName, LastName);

        if (success)
        {
            RegisterSuccessful?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            ErrorMessage = "Ошибка регистрации. Возможно, пользователь уже существует";
        }
    }

    private void GoBack()
    {
        BackRequested?.Invoke(this, EventArgs.Empty);
    }
}
