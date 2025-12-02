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

        public async Task<LoginResult> LoginAsync(string email, string password, CancellationToken ct = default)
        {
            // Create request (attributes in Model will handle casing)
            var req = new LoginRequest { Email = email, Password = password };

            HttpResponseMessage resp;
            try
            {
                resp = await _http.PostAsJsonAsync(ApiConfig.LoginPath, req, ct);
            }
            catch (Exception ex)
            {
                return new LoginResult { Success = false, Message = $"Network error: {ex.Message}" };
            }

            var body = await resp.Content.ReadAsStringAsync(ct);

            if (!resp.IsSuccessStatusCode)
            {
                // Return the actual error from API
                return new LoginResult { Success = false, Message = $"Login failed: {body}" };
            }

            try
            {
                var result = JsonSerializer.Deserialize<LoginResult>(body, JsonOpts) ?? new LoginResult();

                // ROBUST TOKEN CHECK: Check 'Token', 'AccessToken', or 'token'
                string token = result.Token;

                // If the standard property is empty, try to inspect the raw JSON for alternatives
                if (string.IsNullOrWhiteSpace(token))
                {
                    // Fallback logic for different API naming conventions
                    using (JsonDocument doc = JsonDocument.Parse(body))
                    {
                        if (doc.RootElement.TryGetProperty("accessToken", out var t1)) token = t1.GetString();
                        else if (doc.RootElement.TryGetProperty("token", out var t2)) token = t2.GetString();
                    }
                }

                if (!string.IsNullOrWhiteSpace(token))
                {
                    result.Success = true;
                    result.Token = token; // Ensure the main property is set
                    SetAccessToken(token);
                }
                else
                {
                    // If we got a 200 OK but no token, just mark success (some APIs work this way)
                    result.Success = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                return new LoginResult { Success = false, Message = $"Parser error: {ex.Message}" };
            }
        }

        public async Task<LoginResult> RegisterAsync(string email, string password, string firstName, string lastName, CancellationToken ct = default)
        {
            var req = new RegisterRequest
            {
                email = email,
                password = password,
                firstName = firstName,
                lastName = lastName
            };

            HttpResponseMessage resp;
            try
            {
                resp = await _http.PostAsJsonAsync(ApiConfig.RegisterPath, req, ct);
            }
            catch (Exception ex)
            {
                return new LoginResult { Success = false, Message = $"Network error: {ex.Message}" };
            }

            var body = await resp.Content.ReadAsStringAsync(ct);

            if (!resp.IsSuccessStatusCode)
            {
                return new LoginResult { Success = false, Message = body };
            }

            try
            {
                var result = JsonSerializer.Deserialize<LoginResult>(body, JsonOpts) ?? new LoginResult();
                result.Success = true;
                return result;
            }
            catch
            {
                return new LoginResult { Success = true, Message = "Registration successful" };
            }
        }

        public async Task<UserProfile?> GetMeAsync(CancellationToken ct = default)
        {
            try
            {
                var resp = await _http.GetAsync(ApiConfig.MePath, ct);
                if (!resp.IsSuccessStatusCode) return null;
                return await resp.Content.ReadFromJsonAsync<UserProfile>(JsonOpts, ct);
            }
            catch
            {
                return null;
            }
        }
    }
}