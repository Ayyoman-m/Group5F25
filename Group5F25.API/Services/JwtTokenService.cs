using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Group5F25.API.Models;
using Group5F25.API.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Group5F25.API.Services
{
    public interface IJwtTokenService
    {
        string Generate(User user);

        // Optional alias (so old calls like GenerateToken() still work)
        string GenerateToken(User user) => Generate(user);
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtOptions _opts;

        public JwtTokenService(IOptions<JwtOptions> opts)
        {
            _opts = opts.Value ?? new JwtOptions();
        }

        public string Generate(User user)
        {
            // Validate JWT settings
            if (string.IsNullOrWhiteSpace(_opts.Key))
                throw new InvalidOperationException("JWT Key is not configured.");
            if (string.IsNullOrWhiteSpace(_opts.Issuer))
                throw new InvalidOperationException("JWT Issuer is not configured.");
            if (string.IsNullOrWhiteSpace(_opts.Audience))
                throw new InvalidOperationException("JWT Audience is not configured.");

            // ✅ Use your user's ID (or email if ID not available)
            var subject = user.UserId.ToString(); 

            // Create the signing credentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opts.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Define claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, subject),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Create the token
            var token = new JwtSecurityToken(
                issuer: _opts.Issuer,
                audience: _opts.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_opts.ExpiresMinutes <= 0 ? 60 : _opts.ExpiresMinutes),
                signingCredentials: creds
            );

            // Return the token string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
