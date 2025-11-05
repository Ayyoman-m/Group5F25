namespace Group5F25.APP.Models
{
    // Shape of Rasik's AuthResponse
    public sealed class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? UserId { get; set; }
        public string? Token { get; set; }
        public int Code { get; set; }   // matches AuthResultCode enum (as int)

        // Optional: keep compatibility with old code that used accessToken
        public string? accessToken
        {
            get => Token;
            set => Token = value;
        }

        // Optional convenience for your ViewModel
        public string? Error => Success ? null : Message;
    }
}
