namespace Group5F25.APP.Models
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty; // Standardized to "Token"
        public int? UserId { get; set; }
    }
}