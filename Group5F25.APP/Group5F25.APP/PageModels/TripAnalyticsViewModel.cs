using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Group5F25.APP.Models;
using Group5F25.APP.Services;
using Microsoft.Maui.Storage;

namespace Group5F25.APP.PageModels
{
    public partial class TripAnalyticsViewModel : ObservableObject
    {
        private readonly ITripService _tripService;

        [ObservableProperty] private int totalTrips;
        [ObservableProperty] private double averageDurationMinutes;
        [ObservableProperty] private int bestScore;
        [ObservableProperty] private int worstScore;

        [ObservableProperty] private bool isBusy;

        public TripAnalyticsViewModel(ITripService tripService)
        {
            _tripService = tripService;
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;

                var userIdString = Preferences.Get("userId", "0");
                int.TryParse(userIdString, out var userId);

                // You need to implement this in ITripService
                var trips = await _tripService.GetTripsForUserAsync(userId);

                var tripList = trips?.ToList() ?? new List<TripSummary>();

                TotalTrips = tripList.Count;

                if (TotalTrips > 0)
                {
                    AverageDurationMinutes = tripList.Average(t => t.DurationSeconds) / 60.0;
                    BestScore = tripList.Max(t => t.SafetyScore ?? 0);
                    WorstScore = tripList.Min(t => t.SafetyScore ?? 0);
                }
                else
                {
                    AverageDurationMinutes = 0;
                    BestScore = 0;
                    WorstScore = 0;
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
