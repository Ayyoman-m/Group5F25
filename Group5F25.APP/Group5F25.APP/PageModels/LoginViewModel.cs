using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Group5F25.APP.Services;
using System.Diagnostics;

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
            // Valid DummyJSON accounts:
            // emilys / emilyspass  OR  kminchelle / 0lelplR
            LoginEmail = "emilys";
            LoginPassword = "emilyspass";
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
                    ErrorMessage = "Enter username and password.";
                    return;
                }

                try
                {
                    IsBusy = true;
                    var result = await _auth.LoginAsync(email, pass);
                    if (!result.Success || string.IsNullOrWhiteSpace(result.accessToken))
                    {
                        HasError = true;
                        ErrorMessage = result.Error ?? "Invalid credentials.";
                        return;
                    }

                    //var me = await _auth.GetMeAsync();
                    //if (me is null)
                    //{
                    //    HasError = true;
                    //    ErrorMessage = "Token set, but /auth/me failed.";
                    //    return;
                    //}

                    //Debug.WriteLine($"[AUTH VERIFIED] id={me.id} username={me.email}");

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
