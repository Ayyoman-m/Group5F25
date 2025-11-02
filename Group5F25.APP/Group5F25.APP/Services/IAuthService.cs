using Group5F25.APP.Models;

namespace Group5F25.APP.Services
{
    public interface IAuthService
    {
        Task<LoginResult> LoginAsync(string username, string password, CancellationToken ct = default);
        void SetAccessToken(string? token);
        Task<string?> GetMeRawAsync(CancellationToken ct = default); // optional sanity check
    }
}
