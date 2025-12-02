using Group5F25.APP.Models;
using System.Net.Http.Json;

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
            // Call API: api/trip/user/{userId}
            var response = await _httpClient.GetAsync($"{ApiConfig.TripUserBasePath}{userId}", ct);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<TripSummary>>(cancellationToken: ct) ?? new List<TripSummary>();
            }

            return new List<TripSummary>();
        }
    }
}