using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Group5F25.APP.Services;

namespace Group5F25.APP.PageModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        void Raise([CallerMemberName] string? n = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

        private readonly IAuthService _auth;

        public RegisterViewModel(IAuthService auth)
        {
            _auth = auth;

            RegisterCommand = new Command(async () => await OnRegisterAsync(), () => !IsBusy);
            BackToLoginCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync("..");
            });
        }

        // ------------- Fields bound to UI -------------

        private string _firstName = string.Empty;
        public string FirstName
        {
            get => _firstName;
            set { if (_firstName != value) { _firstName = value; Raise(); } }
        }

        private string _lastName = string.Empty;
        public string LastName
        {
            get => _lastName;
            set { if (_lastName != value) { _lastName = value; Raise(); } }
        }

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set { if (_email != value) { _email = value; Raise(); } }
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set { if (_password != value) { _password = value; Raise(); } }
        }

        private string _confirmPassword = string.Empty;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set { if (_confirmPassword != value) { _confirmPassword = value; Raise(); } }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    Raise();
                    Raise(nameof(RegisterButtonText));
                    ((Command)RegisterCommand).ChangeCanExecute();
                }
            }
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

        private bool _hasInfo;
        public bool HasInfo
        {
            get => _hasInfo;
            set { if (_hasInfo != value) { _hasInfo = value; Raise(); } }
        }

        private string _infoMessage = string.Empty;
        public string InfoMessage
        {
            get => _infoMessage;
            set { if (_infoMessage != value) { _infoMessage = value; Raise(); } }
        }

        public string RegisterButtonText => IsBusy ? "Registering..." : "Register";

        public ICommand RegisterCommand { get; }
        public ICommand BackToLoginCommand { get; }

        // ------------- Core logic -------------

        private async Task OnRegisterAsync()
        {
            HasError = false;
            HasInfo = false;
            ErrorMessage = string.Empty;
            InfoMessage = string.Empty;

            var first = (FirstName ?? string.Empty).Trim();
            var last = (LastName ?? string.Empty).Trim();
            var email = (Email ?? string.Empty).Trim();
            var pw = (Password ?? string.Empty).Trim();
            var cpw = (ConfirmPassword ?? string.Empty).Trim();

            // Basic validation before hitting API
            if (string.IsNullOrWhiteSpace(first) ||
                string.IsNullOrWhiteSpace(last) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(pw) ||
                string.IsNullOrWhiteSpace(cpw))
            {
                HasError = true;
                ErrorMessage = "All fields are required.";
                return;
            }

            if (!email.Contains("@"))
            {
                HasError = true;
                ErrorMessage = "Please enter a valid email address.";
                return;
            }

            if (pw.Length < 6)
            {
                HasError = true;
                ErrorMessage = "Password must be at least 6 characters.";
                return;
            }

            if (!string.Equals(pw, cpw, StringComparison.Ordinal))
            {
                HasError = true;
                ErrorMessage = "Passwords do not match.";
                return;
            }

            try
            {
                IsBusy = true;

                var result = await _auth.RegisterAsync(email, pw, first, last);
                if (!result.Success)
                {
                    HasError = true;
                    ErrorMessage = string.IsNullOrWhiteSpace(result.Message)
                        ? "Registration failed."
                        : result.Message!;
                    return;
                }

                // Success path: toast + navigate to success page
                HasInfo = false;                    // optional: no need for inline info on this page now
                InfoMessage = string.Empty;         // optional: clear message

                await AppShell.DisplaySnackbarAsync("Registration successful.");

                // Navigate to the RegistrationSuccessPage (route registered in AppShell as "registrationSuccess")
                await Shell.Current.GoToAsync("registrationSuccess");

            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = $"Network error: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
