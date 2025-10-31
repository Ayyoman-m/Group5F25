using Group5F25.API.Services;

namespace Group5F25.API.Models
{
    // response returned after a user gets registered
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? UserId { get; set; }
        public string? Token { get; set; }
        public AuthResultCode Code { get; set; } = AuthResultCode.Ok;
    }
}
