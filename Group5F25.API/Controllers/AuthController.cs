using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Group5F25.API.Data;
using Group5F25.API.DTOs;
using Group5F25.API.Models;
using Group5F25.API.Services;

namespace Group5F25.API.Controllers
{
    // This controller handles all authentication-related API requests
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly AppDbContext _db;

        // Use dependency injection to get the authentication service and database
        public AuthController(IAuthService auth, AppDbContext db)
        {
            _auth = auth;
            _db = db;
        }

        // POST: /auth/register
        // This method handles new user registration
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            // Check if the data sent by the user is valid
            if (!ModelState.IsValid)
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = "Invalid payload.",
                    Code = AuthResultCode.BadRequest
                });

            // Call the register method in AuthService
            var res = await _auth.RegisterAsync(request);

            // Handle possible errors (duplicate email or bad input)
            if (!res.Success)
                return res.Code == AuthResultCode.Conflict ? Conflict(res) : BadRequest(res);

            // Return success if registration works
            return Ok(res);
        }

        // POST: /auth/login
        // This method handles existing user login
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            // Check if the data sent by the user is valid
            if (!ModelState.IsValid)
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = "Invalid payload.",
                    Code = AuthResultCode.BadRequest
                });

            // Call the login method in AuthService
            var res = await _auth.LoginAsync(request);

            // If login fails, return Unauthorized response
            if (!res.Success)
                return Unauthorized(res);

            // Return success if login is valid
            return Ok(res);
        }

        // GET: /auth/me
        // Returns the currently logged-in user's profile (for the MAUI app)
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserProfileDto>> Me()
        {
            // 1) Get email from JWT claims
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(email))
                return Unauthorized();

            // 2) Load user from database
            var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user is null)
                return NotFound();

            // 3) Map to DTO that matches the MAUI UserProfile model
            var dto = new UserProfileDto
            {
                id = user.UserId,
                username = user.Email,      // or any username you want
                email = user.Email,
                firstName = user.FirstName,
                lastName = user.LastName,
                role = "Driver"         // change if you have real roles
            };

            return Ok(dto);
        }
    }
}
