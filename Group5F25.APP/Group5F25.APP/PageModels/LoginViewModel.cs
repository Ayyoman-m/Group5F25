using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;           // <-- IMPORTANT
using Group5F25.APP.Services;

namespace Group5F25.APP.PageModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        void Raise([CallerMemberName] string? n = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

        private readonly IAuthService _auth;

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
            // Default demo credentials
            LoginEmail = "test@example.com";
            LoginPassword = "password123";
#endif

            LoginCommand = new Command(async () =>
            {
                HasError = false;
                ErrorMessage = string.Empty;

                var email = (LoginEmail ?? string.Empty).Trim();
                var pass = (LoginPassword ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass))
                {
                    HasError = true;
                    ErrorMessage = "Enter email and password.";
                    return;
                }

                try
                {
                    IsBusy = true;

                    // 1) LOGIN → get token & set Authorization header inside AuthService
                    var result = await _auth.LoginAsync(email, pass);
                    if (!result.Success || string.IsNullOrWhiteSpace(result.accessToken))
                    {
                        HasError = true;
                        // Use Message because LoginResult in AuthService sets Message, not Error
                        ErrorMessage = string.IsNullOrWhiteSpace(result.Message)
                            ? "Invalid credentials."
                            : result.Message;
                        return;
                    }

                    // 2) /auth/me → get full profile with firstName / lastName
                    var me = await _auth.GetMeAsync();
                    if (me == null)
                    {
                        // if /auth/me fails, at least store what user typed
                        Preferences.Set("displayName", email);
                        Preferences.Set("username", email);
                    }
                    else
                    {
                        // Build full name
                        var fullName = $"{me.firstName} {me.lastName}".Trim();

                        // If first/last are empty (e.g., old users without names), fall back
                        if (string.IsNullOrWhiteSpace(fullName))
                        {
                            fullName = string.IsNullOrWhiteSpace(me.username)
                                ? email
                                : me.username;
                        }

                        Preferences.Set("displayName", fullName);
                        Preferences.Set("username", string.IsNullOrWhiteSpace(me.username) ? email : me.username);
                        Preferences.Set("userId", me.id.ToString());
                        Preferences.Set("userRole", me.role ?? "");
                    }

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
