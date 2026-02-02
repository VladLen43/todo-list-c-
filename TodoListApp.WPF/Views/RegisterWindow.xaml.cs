using System.Windows;
using TodoListApp.WPF.Services;
using TodoListApp.WPF.ViewModels;

namespace TodoListApp.WPF.Views;

public partial class RegisterWindow : Window
{
    private readonly RegisterViewModel _viewModel;

    public RegisterWindow()
    {
        InitializeComponent();
        _viewModel = new RegisterViewModel(new ApiService());
        DataContext = _viewModel;

        _viewModel.RegisterSuccessful += (s, e) =>
        {
            DialogResult = true;
            Close();
        };

        _viewModel.BackRequested += (s, e) =>
        {
            DialogResult = false;
            Close();
        };
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (sender is System.Windows.Controls.PasswordBox passwordBox)
        {
            _viewModel.Password = passwordBox.Password;
        }
    }

    private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (sender is System.Windows.Controls.PasswordBox passwordBox)
        {
            _viewModel.ConfirmPassword = passwordBox.Password;
        }
    }
}
