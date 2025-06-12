using My_Pos.Commands;
using System.ComponentModel;
using System.Windows.Input;
using System;

namespace My_Pos.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public ICommand LoginCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? LoginSucceeded;

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        private void ExecuteLogin(object? parameter)
        {
            // No message box, no validation
            LoginSucceeded?.Invoke(this, EventArgs.Empty);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
