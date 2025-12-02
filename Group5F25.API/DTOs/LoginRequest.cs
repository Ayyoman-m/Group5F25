using System.ComponentModel.DataAnnotations;

namespace Group5F25.API.DTOs
{
    // used when user logs into system
    public class LoginRequest
    {
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
    }
}
