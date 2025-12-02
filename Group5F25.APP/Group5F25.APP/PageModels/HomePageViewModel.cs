using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Dispatching;
using System.Windows.Input;
using Group5F25.APP.Models;
using Group5F25.APP.Services;

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
        private string userName = "Birendra";

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

        public ICommand NavigateToTripHistoryCommand { get; }


        public HomePageViewModel(IDispatcher dispatcher, ITripService tripService)
        {
            _dispatcher = dispatcher;

            _tripService = tripService;

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

            // TODO: replace 1 with the real logged-in user id when you have it.
            var upload = new TripUploadRequest
            {
                UserId = 1,
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