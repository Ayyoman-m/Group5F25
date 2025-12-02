using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Group5F25.APP.Models;
using Group5F25.APP.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group5F25.APP.PageModels
{
    public partial class LeaderboardViewModel : ObservableObject
    {
        private readonly ITripService _tripService;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private List<LeaderboardEntry> entries = new();

        public LeaderboardViewModel(ITripService tripService)
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

                // For now we just call a leaderboard API; implement in ITripService later.
                var data = await _tripService.GetLeaderboardAsync();
                Entries = data.OrderBy(e => e.Rank).ToList();
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
