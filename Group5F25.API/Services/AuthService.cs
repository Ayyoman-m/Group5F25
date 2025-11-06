using Microsoft.EntityFrameworkCore;
using Group5F25.API.Data;
using Group5F25.API.DTOs;
using Group5F25.API.Models;

namespace Group5F25.API.Services
{
    // Handles authentication using MySQL for users and InMemory DB for analytics
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly DriverAnalyticsContext _analyticsDb;
        private readonly IJwtTokenService _jwt;

        // Constructor: connects to database and JWT service
        public AuthService(AppDbContext context, DriverAnalyticsContext analyticsDb, IJwtTokenService jwt)
        {
            _context = context;
            _analyticsDb = analyticsDb;
            _jwt = jwt;
        }


        // Registers a new user
        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            // Check if email or password is empty
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return new AuthResponse { Success = false, Message = "Email and Password are required.", Code = AuthResultCode.BadRequest };

            // Check if the email already exists in the database
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return new AuthResponse { Success = false, Message = "Email already registered.", Code = AuthResultCode.Conflict };

            // Create a new user and hash their password
            var user = new User
            {
                FirstName = request.FirstName ?? string.Empty,
                LastName = request.LastName ?? string.Empty,
                Email = request.Email,
                PasswordHash = PasswordHasher.HashPassword(request.Password), // Securely hash password
                CreatedAt = DateTime.UtcNow
            };

            // Add user to database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Optional: record registration event in analytics database
            // _analyticsDb.UserEvents.Add(new UserEvent { UserId = user.UserId, EventType = "Register", Timestamp = DateTime.UtcNow });
            // await _analyticsDb.SaveChangesAsync();


            // Generate a JWT token for the new user
            var token = _jwt.GenerateToken(user);

            // Return success response
            return new AuthResponse
            {
                Success = true,
                Message = "Registration successful.",
                Token = token,
                UserId = user.UserId,
                Code = AuthResultCode.Ok
            };
        }

        // Logs in an existing user
        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            // Check for empty fields
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return new AuthResponse { Success = false, Message = "Email and Password are required.", Code = AuthResultCode.BadRequest };

            // Find the user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                return new AuthResponse { Success = false, Message = "Invalid email or password.", Code = AuthResultCode.Unauthorized };

            // Verify the password
            var ok = PasswordHasher.VerifyPassword(request.Password, user.PasswordHash);
            if (!ok)
                return new AuthResponse { Success = false, Message = "Invalid email or password.", Code = AuthResultCode.Unauthorized };

            // Generate JWT token for successful login
            var token = _jwt.GenerateToken(user);

            // Optional: record login event
            // _analyticsDb.UserEvents.Add(new UserEvent { UserId = user.UserId, EventType = "Login", Timestamp = DateTime.UtcNow });
            // await _analyticsDb.SaveChangesAsync();


            // Return success response
            return new AuthResponse
            {
                Success = true,
                Message = "Login successful.",
                Token = token,
                UserId = user.UserId,
                Code = AuthResultCode.Ok
            };
        }
    }
}
