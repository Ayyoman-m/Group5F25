using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Group5F25.APP.Services;

namespace Group5F25.APP.PageModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        void Raise([CallerMemberName] string? n = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

        private readonly IAuthService _auth;

        private string _loginUsername = string.Empty;
        public string LoginUsername
        {
            get => _loginUsername;
            set { if (_loginUsername != value) { _loginUsername = value; Raise(); } }
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
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    Raise();
                    Raise(nameof(LoginButtonText));
                    ((Command)LoginCommand).ChangeCanExecute();
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

        public string LoginButtonText => IsBusy ? "Signing in..." : "Login";
        public ICommand LoginCommand { get; }

        public LoginViewModel(IAuthService auth)
        {
            _auth = auth;

#if DEBUG
            // Valid DummyJSON accounts:
            // emilys / emilyspass  OR  kminchelle / 0lelplR
            LoginUsername = "emilys";
            LoginPassword = "emilyspass";
#endif

            LoginCommand = new Command(async () =>
            {
                HasError = false;
                ErrorMessage = string.Empty;

                var user = (LoginUsername ?? string.Empty).Trim();
                var pass = (LoginPassword ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
                {
                    HasError = true;
                    ErrorMessage = "Enter username and password.";
                    return;
                }

                try
                {
                    IsBusy = true;
                    var result = await _auth.LoginAsync(user, pass);
                    if (!result.Success || string.IsNullOrWhiteSpace(result.accessToken))
                    {
                        HasError = true;
                        ErrorMessage = result.Error ?? "Invalid credentials.";
                        return;
                    }

                    // Optional sanity call to /auth/me using the bearer set in AuthService
                    var me = await _auth.GetMeRawAsync();
                    if (me is null)
                    {
                        HasError = true;
                        ErrorMessage = "Token set, but /auth/me failed.";
                        return;
                    }

                    // SUCCESS → navigate to HomePage
                    await AppShell.DisplayToastAsync("Signed in successfully.");
                    await Shell.Current.GoToAsync("//home");
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
            }, () => !IsBusy);
        }
    }
}
