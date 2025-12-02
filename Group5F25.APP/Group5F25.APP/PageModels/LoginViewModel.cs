using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Group5F25.APP.Models;
using Group5F25.APP.Services;

namespace Group5F25.APP.PageModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        // FIX: Added IsLoading to control button visibility/activity indicator
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotLoading))]
        private bool isLoading;

        public bool IsNotLoading => !IsLoading;

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        private async Task Login()
        {
            if (IsLoading) return; // Prevent double clicks

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Error", "Please enter email and password", "OK");
                return;
            }

            IsLoading = true; // Hide Button, Show Spinner

            try
            {
                var result = await _authService.LoginAsync(Email, Password);

                if (result != null && result.Success)
                {
                    Preferences.Set("UserId", result.UserId ?? 0);
                    Preferences.Set("UserName", Email);
                    Preferences.Set("AuthToken", result.Token);

                    await Shell.Current.GoToAsync("//home");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", result?.Message ?? "Login Failed", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Connection failed: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false; // Show Button again
            }
        }

        [RelayCommand]
        private async Task GoToRegister()
        {
            await Shell.Current.GoToAsync("register");
        }
    }
}