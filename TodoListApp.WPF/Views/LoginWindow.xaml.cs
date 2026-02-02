using System.Windows;
using TodoListApp.WPF.ViewModels;

namespace TodoListApp.WPF.Views;

public partial class LoginWindow : Window
{
    private readonly LoginViewModel _viewModel;

    public LoginWindow(LoginViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;

        _viewModel.LoginSuccessful += (s, e) =>
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        };

        _viewModel.RegisterRequested += (s, e) =>
        {
            var registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
            if (registerWindow.DialogResult == true)
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
        };
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (sender is System.Windows.Controls.PasswordBox passwordBox)
        {
            _viewModel.Password = passwordBox.Password;
        }
    }
}
