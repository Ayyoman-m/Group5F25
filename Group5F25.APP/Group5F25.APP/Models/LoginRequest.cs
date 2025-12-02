using System.Text.Json.Serialization;

namespace Group5F25.APP.Models
{
    public sealed class LoginRequest
    {
        // FIX: Force lowercase JSON keys to match API expectations perfectly
        [JsonPropertyName("email")]
        public string Email { get; set; } = "";

        [JsonPropertyName("password")]
        public string Password { get; set; } = "";
    }
}