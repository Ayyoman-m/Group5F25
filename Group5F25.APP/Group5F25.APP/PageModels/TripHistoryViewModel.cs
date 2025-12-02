using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
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

        public ObservableCollection<TripSummary> Trips { get; } =
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

        public ICommand RefreshCommand { get; }

        public TripHistoryViewModel(ITripService tripService)
        {
            _tripService = tripService;
            RefreshCommand = new Command(async () => await LoadTripsAsync(), () => !IsBusy);
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

                var trips = await _tripService.GetTripsAsync(1);


                foreach (var t in trips)
                {
                    Trips.Add(t);
                }
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
    }
}
