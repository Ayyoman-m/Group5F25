using Group5F25.APP.Models;

namespace Group5F25.APP.Services
{
    public interface ITripService
    {
        Task UploadTripAsync(TripUploadRequest trip, CancellationToken ct = default);

        // NEW — this is why your TripHistory was failing
        Task<List<TripSummary>> GetTripsAsync(int userId, CancellationToken ct = default);

        Task<IEnumerable<TripSummary>> GetTripsForUserAsync(int userId, CancellationToken ct = default);

        Task<IEnumerable<LeaderboardEntry>> GetLeaderboardAsync();


    }
}
