namespace Group5F25.APP.Models
{
    // Shape of DummyJSON login response (simplified)
    public sealed class LoginResult
    {
        public bool Success { get; set; }
        public string? Error { get; set; }

        // DummyJSON returns "accessToken" and "refreshToken"
        public string? accessToken { get; set; }
        public string? refreshToken { get; set; }

        // Convenience property used by your VM
        public string? Token
        {
            get => accessToken;
            set => accessToken = value;
        }
    }
}
