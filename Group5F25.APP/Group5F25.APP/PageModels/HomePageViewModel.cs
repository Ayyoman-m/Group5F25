using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Dispatching;
using System.Windows.Input;
using Group5F25.APP.Models;
using Group5F25.APP.Services;
using Microsoft.Maui.Devices.Sensors;

namespace Group5F25.APP.PageModels
{
    public partial class HomePageViewModel : ObservableObject
    {
        private readonly IDispatcher _dispatcher;
        private readonly ITripService _tripService;

        private IDispatcherTimer? _timer;
        private DateTime _startTime;
        private List<TripDataPoint> _currentTripData = new();

        [ObservableProperty]
        private string userName; // Name is now dynamic

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
            NavigateToTripHistoryCommand = new AsyncRelayCommand(OnNavigateToTripHistoryAsync);

            // Load the name from login
            UserName = Preferences.Get("UserName", "Driver");
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
                await StartTripAsync();
            }
        }

        private async Task StartTripAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    await Shell.Current.DisplayAlert("Permission Required", "Location permission is needed.", "OK");
                    return;
                }
            }

            _currentTripData.Clear();
            _startTime = DateTime.Now;
            IsRecording = true;
            ActionButtonText = "Stop Trip";
            ActionButtonColor = Color.FromArgb("#dc3545"); // Red

            if (_timer == null)
            {
                _timer = _dispatcher.CreateTimer();
                _timer.Interval = TimeSpan.FromSeconds(1);
                _timer.Tick += (s, e) => OnTimerTick();
            }
            _timer.Start();
            ToggleSensors(true);
        }

        private async Task StopTripAsync()
        {
            IsRecording = false;
            _timer?.Stop();
            ToggleSensors(false);

            var endTime = DateTime.Now;
            var duration = endTime - _startTime;
            TimerText = duration.ToString(@"hh\:mm\:ss");

            // GET REAL USER ID
            int userId = Preferences.Get("UserId", 1); // Default to 1 if missing

            var upload = new TripUploadRequest
            {
                UserId = userId,
                StartTime = _startTime,
                EndTime = endTime,
                DurationSeconds = (int)duration.TotalSeconds,
                SafetyScore = OverallScore,
                Notes = "Mobile Trip",
                DataPoints = new List<TripDataPoint>(_currentTripData)
            };

            try
            {
                await _tripService.UploadTripAsync(upload);
                await Shell.Current.DisplayAlert("Trip Finished", $"Total Time: {TimerText}\nTrip uploaded successfully!", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Upload failed: {ex.Message}", "OK");
            }

            TimerText = "00:00:00";
            ActionButtonText = "Start Trip";
            ActionButtonColor = Color.FromArgb("#28a745");
            _currentTripData.Clear();
        }

        private void ToggleSensors(bool start)
        {
            if (start)
            {
                if (Accelerometer.Default.IsSupported && !Accelerometer.Default.IsMonitoring)
                {
                    Accelerometer.Default.ReadingChanged += OnAccelerometerReadingChanged;
                    Accelerometer.Default.Start(SensorSpeed.UI);
                }

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    if (Geolocation.Default.IsListeningForeground) return;
                    Geolocation.Default.LocationChanged += OnLocationChanged;
                    await Geolocation.Default.StartListeningForegroundAsync(new GeolocationListeningRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(5)));
                });
            }
            else
            {
                if (Accelerometer.Default.IsSupported)
                {
                    Accelerometer.Default.ReadingChanged -= OnAccelerometerReadingChanged;
                    Accelerometer.Default.Stop();
                }
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Geolocation.Default.LocationChanged -= OnLocationChanged;
                    Geolocation.Default.StopListeningForeground();
                });
            }
        }

        private void OnAccelerometerReadingChanged(object? sender, AccelerometerChangedEventArgs e)
        {
            _currentTripData.Add(new TripDataPoint
            {
                Timestamp = DateTime.UtcNow,
                AccelerationX = e.Reading.Acceleration.X,
                AccelerationY = e.Reading.Acceleration.Y,
                AccelerationZ = e.Reading.Acceleration.Z
            });
        }

        private void OnLocationChanged(object? sender, GeolocationLocationChangedEventArgs e)
        {
            var location = e.Location;
            if (location != null)
            {
                _currentTripData.Add(new TripDataPoint
                {
                    Timestamp = DateTime.UtcNow,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    Speed = location.Speed ?? 0
                });
            }
        }

        private void OnTimerTick()
        {
            var duration = DateTime.Now - _startTime;
            TimerText = duration.ToString(@"hh\:mm\:ss");
        }
    }
}