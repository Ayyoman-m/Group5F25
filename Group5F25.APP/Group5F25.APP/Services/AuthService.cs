using System.Net.Http.Json;
using System.Text.Json;
using Group5F25.APP.Models;

namespace Group5F25.APP.Services
{
    public sealed class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

        public AuthService(HttpClient http)
        {
            _http = http;
            if (_http.BaseAddress is null)
                _http.BaseAddress = new Uri(ApiConfig.BaseUrl);
        }

        public void SetAccessToken(string? token)
        {
            _http.DefaultRequestHeaders.Remove("Authorization");
            if (!string.IsNullOrWhiteSpace(token))
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        public async Task<LoginResult> LoginAsync(string username, string password, CancellationToken ct = default)
        {
            var req = new LoginRequest { username = username, password = password };

            HttpResponseMessage resp;
            try
            {
                resp = await _http.PostAsJsonAsync(ApiConfig.LoginPath, req, ct);
            }
            catch (Exception ex)
            {
                return new LoginResult { Success = false, Error = $"Network error: {ex.Message}" };
            }

            var body = await resp.Content.ReadAsStringAsync(ct);

            if (!resp.IsSuccessStatusCode)
                return new LoginResult { Success = false, Error = $"Login failed ({(int)resp.StatusCode}). Body: {body}" };

            // Parse expected DummyJSON fields
            var result = JsonSerializer.Deserialize<LoginResult>(body, JsonOpts) ?? new LoginResult();
            if (string.IsNullOrWhiteSpace(result.accessToken))
                return new LoginResult { Success = false, Error = "Unexpected response format. accessToken missing." };

            result.Success = true;
            SetAccessToken(result.accessToken);
            return result;
        }

        public async Task<string?> GetMeRawAsync(CancellationToken ct = default)
        {
            var resp = await _http.GetAsync(ApiConfig.MePath, ct);
            if (!resp.IsSuccessStatusCode) return null;
            return await resp.Content.ReadAsStringAsync(ct);
        }

        public async Task<UserProfile?> GetMeAsync(CancellationToken ct = default)
        {
            var resp = await _http.GetAsync(ApiConfig.MePath, ct);
            if (!resp.IsSuccessStatusCode) return null;
            return await resp.Content.ReadFromJsonAsync<UserProfile>(JsonOpts, ct);
        }
        
    }
}
