using Microsoft.AspNetCore.Mvc;
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

        // Use dependency injection to get the authentication service
        public AuthController(IAuthService auth) => _auth = auth;

        // POST: /auth/register
        // This method handles new user registration
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            // Check if the data sent by the user is valid
            if (!ModelState.IsValid)
                return BadRequest(new AuthResponse { Success = false, Message = "Invalid payload.", Code = AuthResultCode.BadRequest });

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
                return BadRequest(new AuthResponse { Success = false, Message = "Invalid payload.", Code = AuthResultCode.BadRequest });

            // Call the login method in AuthService
            var res = await _auth.LoginAsync(request);

            // If login fails, return Unauthorized response
            if (!res.Success) return Unauthorized(res);

            // Return success if login is valid
            return Ok(res);
        }
    }
}
