using Group5F25.APP.Models;

namespace Group5F25.APP.Services
{
    public interface IAuthService
    {
        // LOGIN (used by LoginViewModel)
        Task<LoginResult> LoginAsync(string email, string password, CancellationToken ct = default);

        // REGISTER (used by RegisterViewModel)
        Task<LoginResult> RegisterAsync(string email, string password, string firstName, string lastName, CancellationToken ct = default);

        // Used internally to attach Authorization: Bearer <token>
        void SetAccessToken(string? token);

        // Stubbed (optional) — used before we disabled /auth/me
        Task<UserProfile?> GetMeAsync(CancellationToken ct = default);
    }
}
