using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Linq;
using Group5F25.APP.Models;
using Group5F25.APP.Services;

namespace Group5F25.APP.PageModels
{
    public class TripHistoryViewModel : INotifyPropertyChanged
    {
        private readonly ITripService _tripService;

        public event PropertyChangedEventHandler? PropertyChanged;
        void Raise([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // All trips from API
        public ObservableCollection<TripSummary> Trips { get; } =
            new ObservableCollection<TripSummary>();

        // Trips after applying filter (bound to UI)
        public ObservableCollection<TripSummary> FilteredTrips { get; } =
            new ObservableCollection<TripSummary>();

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
                    ((Command)RefreshCommand).ChangeCanExecute();
                }
            }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    Raise();
                }
            }
        }

        // Current selected filter
        private string _selectedFilter = "All";
        public string SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                if (_selectedFilter != value)
                {
                    _selectedFilter = value;
                    Raise();
                    ApplyFilter();
                }
            }
        }

        public ICommand RefreshCommand { get; }
        public ICommand ChangeFilterCommand { get; }

        public TripHistoryViewModel(ITripService tripService)
        {
            _tripService = tripService;

            RefreshCommand = new Command(async () => await LoadTripsAsync(), () => !IsBusy);

            ChangeFilterCommand = new Command<string>(mode =>
            {
                if (!string.IsNullOrWhiteSpace(mode))
                    SelectedFilter = mode;
            });

            SelectedFilter = "All";
        }

        public async Task LoadTripsAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;
                Trips.Clear();

                // TODO: replace 1 with actual logged-in user id when you hook it up
                var trips = await _tripService.GetTripsAsync(1);

                foreach (var t in trips)
                    Trips.Add(t);

                ApplyFilter();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Unable to load trips: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ApplyFilter()
        {
            FilteredTrips.Clear();

            IEnumerable<TripSummary> query = Trips;
            var now = DateTime.Now;

            if (SelectedFilter == "Last 7 Days")
            {
                var from = now.AddDays(-7);
                query = query.Where(t => t.StartTime >= from);
            }
            else if (SelectedFilter == "Last 30 Days")
            {
                var from = now.AddDays(-30);
                query = query.Where(t => t.StartTime >= from);
            }
            // "All" = no extra condition

            foreach (var trip in query.OrderByDescending(t => t.StartTime))
                FilteredTrips.Add(trip);
        }
    }
}
