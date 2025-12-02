using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Group5F25.APP.Models;
using Group5F25.APP.Services;
using System.Collections.ObjectModel;

namespace Group5F25.APP.PageModels
{
    public partial class TripHistoryViewModel : ObservableObject
    {
        private readonly ITripService _tripService;

        public ObservableCollection<TripSummary> Trips { get; } = new();

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private bool isEmpty;

        public TripHistoryViewModel(ITripService tripService)
        {
            _tripService = tripService;
        }

        // FIX: Method is now public and named LoadTripsAsync
        [RelayCommand]
        public async Task LoadTripsAsync()
        {
            if (IsLoading) return;

            IsLoading = true;
            try
            {
                int userId = Preferences.Get("UserId", 0);

                if (userId == 0)
                {
                    Trips.Clear();
                    IsEmpty = true;
                    return;
                }

                var tripList = await _tripService.GetTripsAsync(userId);

                Trips.Clear();
                foreach (var trip in tripList)
                {
                    Trips.Add(trip);
                }

                IsEmpty = Trips.Count == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading trips: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}