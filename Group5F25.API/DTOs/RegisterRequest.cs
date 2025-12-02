using System.ComponentModel.DataAnnotations;

namespace Group5F25.API.DTOs
{

    // used when a new user registers in the system
    public class RegisterRequest
    {
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
        [Required] public string FirstName { get; set; } = string.Empty;
        [Required] public string LastName { get; set; } = string.Empty;
    }
}
