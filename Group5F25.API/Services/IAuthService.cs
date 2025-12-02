using System.Threading.Tasks;
using Group5F25.API.DTOs;
using Group5F25.API.Models;

namespace Group5F25.API.Services
{
    // This interface defines what actions our authentication service must have
    public interface IAuthService
    {
        // Method for registering a new user
        Task<AuthResponse> RegisterAsync(RegisterRequest request);

        // Method for logging in an existing user
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }

    // Common response codes for authentication results
    public enum AuthResultCode
    {
        Ok = 200,              // Success
        BadRequest = 400,      // Missing or invalid input
        Unauthorized = 401,    // Wrong email or password
        Conflict = 409         // Email already exists
    }
}
