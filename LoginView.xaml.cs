using System;
using System.Windows;
using My_Pos.ViewModels;

namespace My_Pos.Views
{
    public partial class LoginView : Window
    {
        private readonly LoginViewModel viewModel;

        public LoginView()
        {
            InitializeComponent();

            viewModel = new LoginViewModel();
            this.DataContext = viewModel;
            viewModel.LoginSucceeded += OnLoginSucceeded;
        }

        private void OnLoginSucceeded(object? sender, EventArgs e)
        {
            ProductView dashboard = new ProductView();
            dashboard.Show();

            // Set new MainWindow so app doesn’t exit when Login window closes
            Application.Current.MainWindow = dashboard;

            this.Close(); // ✅ Close login window
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // You can leave this empty for now
        }
    }
}
