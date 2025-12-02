using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using System.Data;

namespace Group5F25.APP.PageModels
{
    public partial class ProfilePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string fullName;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string role;

        [ObservableProperty]
        private string initials;

        public ProfilePageViewModel()
        {
            LoadFromPreferences();
        }

        public void LoadFromPreferences()
        {
            var name = Preferences.Get("displayName",
                       Preferences.Get("username", "User"));
            var emailPref = Preferences.Get("username", "unknown@example.com");
            var rolePref = Preferences.Get("userRole", "Driver");

            FullName = name;
            Email = emailPref;
            Role = rolePref;
            Initials = GetInitials(name);
        }

        private string GetInitials(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "U";

            var parts = name
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Take(2)
                .ToArray();

            if (parts.Length == 1)
                return char.ToUpper(parts[0][0]).ToString();

            return $"{char.ToUpper(parts[0][0])}{char.ToUpper(parts[1][0])}";
        }

        [RelayCommand]
        private async Task EditProfile()
        {
            // For now just a placeholder
            await Shell.Current.DisplayAlert("Edit Profile",
                "Editing profile is not implemented yet.", "OK");
        }

        [RelayCommand]
        private async Task ChangePassword()
        {
            await Shell.Current.DisplayAlert("Change Password",
                "Change password is not implemented yet.", "OK");
        }
    }
}
