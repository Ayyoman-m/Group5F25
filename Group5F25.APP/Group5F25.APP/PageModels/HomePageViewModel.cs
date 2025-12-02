using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Dispatching;
using System.Windows.Input;
using Group5F25.APP.Models;
using Group5F25.APP.Services;
using Microsoft.Maui.Storage;

namespace Group5F25.APP.PageModels
{
    public partial class HomePageViewModel : ObservableObject
    {
        private readonly IDispatcher _dispatcher;
        private readonly ITripService _tripService;

        // FIX: Added '?' to make it nullable. 
        // This tells the compiler "It's okay if this is null at the start".
        private IDispatcherTimer? _timer;

        private DateTime _startTime;

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string userInitials;

        [ObservableProperty]
        private int overallScore = 85;

        [ObservableProperty]
        private string scoreMessage = "Excellent";

        [ObservableProperty]
        private bool isRecording;

        [ObservableProperty]
        private string timerText = "00:00:00";

        [ObservableProperty]
        private string actionButtonText = "Start Trip";

        [ObservableProperty]
        private Color actionButtonColor = Color.FromArgb("#28a745"); // Green

        [RelayCommand]
        private async Task Logout()
        {
            // Clear stored user info
            Preferences.Clear();

            // Navigate back to login page route
            await Shell.Current.GoToAsync("//login");
        }

        [RelayCommand]
        private async Task OpenProfile()
        {
            await Shell.Current.GoToAsync("profile");
        }

        [RelayCommand]
        private async Task NavigateToAnalytics()
        {
            await Shell.Current.GoToAsync("analytics");
        }

        [RelayCommand]
        private async Task NavigateToLeaderboard()
        {
            await Shell.Current.GoToAsync("leaderboard");
        }


        public ICommand NavigateToTripHistoryCommand { get; }


        public HomePageViewModel(IDispatcher dispatcher, ITripService tripService)
        {
            _dispatcher = dispatcher;
            _tripService = tripService;

            // Prefer full display name, then username, then "User"
            var name = Preferences.Get("displayName",
                       Preferences.Get("username", "User"));

            UserName = name;
            UserInitials = GetInitials(name);

            NavigateToTripHistoryCommand = new AsyncRelayCommand(OnNavigateToTripHistoryAsync);
        }

        // PUBLIC helper so the page can refresh name + initials any time
        public void RefreshUser()
        {
            var name = Preferences.Get("displayName",
                       Preferences.Get("username", "User"));

            UserName = name;
            UserInitials = GetInitials(name);
        }

        // Helper method in HomePageViewModel (can be private)
        private string GetInitials(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "U";

            var parts = name
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Take(2)
                .ToArray();

            if (parts.Length == 0)
                return "U";

            if (parts.Length == 1)
                return char.ToUpper(parts[0][0]).ToString();

            return $"{char.ToUpper(parts[0][0])}{char.ToUpper(parts[1][0])}";
        }


        private async Task OnNavigateToTripHistoryAsync()
        {
            await Shell.Current.GoToAsync("tripHistory");
        }


        [RelayCommand]
        private async Task ToggleTrip()
        {
            if (IsRecording)
            {
                await StopTripAsync();
            }
            else
            {
                StartTrip();
            }
        }

        private void StartTrip()
        {
            IsRecording = true;
            _startTime = DateTime.Now;

            ActionButtonText = "Stop Trip";
            ActionButtonColor = Color.FromArgb("#dc3545"); // Red

            // Lazy initialization: Create the timer only when needed
            if (_timer == null)
            {
                _timer = _dispatcher.CreateTimer();
                _timer.Interval = TimeSpan.FromSeconds(1);
                _timer.Tick += (s, e) => OnTimerTick();
            }

            _timer.Start();
        }

        private async Task StopTripAsync()
        {
            IsRecording = false;
            _timer?.Stop();

            var endTime = DateTime.Now;
            var duration = endTime - _startTime;
            TimerText = duration.ToString(@"hh\:mm\:ss");

            // Read userId from Preferences (set in LoginViewModel)
            var userIdString = Preferences.Get("userId", "0");
            int.TryParse(userIdString, out var parsedUserId);

            var upload = new TripUploadRequest
            {
                UserId = parsedUserId, // real logged-in user id
                StartTime = _startTime,
                EndTime = endTime,
                DurationSeconds = (int)duration.TotalSeconds,
                SafetyScore = OverallScore,
                Notes = null
            };

            try
            {
                await _tripService.UploadTripAsync(upload);
                await Shell.Current.DisplayAlert("Trip Finished",
                    $"Total Time: {TimerText}\nTrip uploaded successfully.", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Trip Finished",
                    $"Total Time: {TimerText}\nBut upload failed: {ex.Message}", "OK");
            }

            TimerText = "00:00:00";
            ActionButtonText = "Start Trip";
            ActionButtonColor = Color.FromArgb("#28a745"); // Green
        }


        private void OnTimerTick()
        {
            var duration = DateTime.Now - _startTime;
            TimerText = duration.ToString(@"hh\:mm\:ss");
        }

    }
}