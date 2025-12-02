using System.Net.Http.Json;
using Group5F25.APP.Models;

namespace Group5F25.APP.Services
{
    public class TripService : ITripService
    {
        private readonly HttpClient _httpClient;

        public TripService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task UploadTripAsync(TripUploadRequest trip, CancellationToken ct = default)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiConfig.TripUploadPath, trip, ct);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<TripSummary>> GetTripsAsync(int userId, CancellationToken ct = default)
        {
            var path = ApiConfig.TripUserBasePath + userId;

            var response = await _httpClient.GetAsync(path, ct);
            response.EnsureSuccessStatusCode();

            var trips = await response.Content.ReadFromJsonAsync<List<TripSummary>>(cancellationToken: ct);
            return trips ?? new List<TripSummary>();
        }

    }
}
