using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace Group5F25.APP.PageModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        void Raise([CallerMemberName] string? n = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

        private string _loginEmail = string.Empty;
        public string LoginEmail
        {
            get => _loginEmail;
            set { if (_loginEmail != value) { _loginEmail = value; Raise(); } }
        }

        private string _loginPassword = string.Empty;
        public string LoginPassword
        {
            get => _loginPassword;
            set { if (_loginPassword != value) { _loginPassword = value; Raise(); } }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set { if (_isBusy != value) { _isBusy = value; Raise(); Raise(nameof(LoginButtonText)); } }
        }

        private bool _hasError;
        public bool HasError
        {
            get => _hasError;
            set { if (_hasError != value) { _hasError = value; Raise(); } }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { if (_errorMessage != value) { _errorMessage = value; Raise(); } }
        }

        public string LoginButtonText => IsBusy ? "Signing in..." : "Login";

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            // For Task 1: UI-only behavior (no API). Just validate and show an error if empty.
            LoginCommand = new Command(async () =>
            {
                HasError = false; ErrorMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(LoginEmail) || string.IsNullOrWhiteSpace(LoginPassword))
                {
                    HasError = true; ErrorMessage = "Please enter email and password.";
                    return;
                }

                IsBusy = true;
                await Task.Delay(500); // simulate a quick processing feel
                IsBusy = false;

                // Navigation will be added in Task 2 (after API result). For Task 1, we stop here.
            }, () => !IsBusy);
        }
    }
}
