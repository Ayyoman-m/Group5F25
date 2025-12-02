using Group5F25.API.Data;
using Group5F25.API.Services;
using Group5F25.API.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
//using MySql.Data.MySqlClient; // Needed for the old code, keep it in case you revert.

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------
//  1. Configure JwtOptions from appsettings
// -----------------------------------------------------
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

// -----------------------------------------------------
//  2. DATABASE: Use In-Memory for local dev (Quick Fix)
// -----------------------------------------------------

// 2a. DriverAnalyticsContext (for Trips/Scores)
// Use in-memory database for fast, temporary testing of trip data.
builder.Services.AddDbContext<DriverAnalyticsContext>(opts =>
    opts.UseInMemoryDatabase("DriverAnalytics_Local"));

// 2b. AppDbContext (for Auth/Users) - QUICK FIX: NOW IN-MEMORY
// This bypasses the failing MySQL connection for user registration/login.
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseInMemoryDatabase("Auth_Local"));


// -----------------------------------------------------
//  3. DEPENDENCY INJECTION (DI)
// -----------------------------------------------------
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();

// -----------------------------------------------------
//  4. CONTROLLERS, SWAGGER, and CORS
// -----------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(p => p.AddDefaultPolicy(policy =>
    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()
));

// -----------------------------------------------------
//  5. JWT AUTHENTICATION
// -----------------------------------------------------
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "Group5F25.API";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "Group5F25.Client";

// Fallback key if missing (for testing)
if (string.IsNullOrWhiteSpace(jwtKey))
{
    if (builder.Environment.IsEnvironment("Testing"))
    {
        jwtKey = "TEST_SECRET_KEY_SHOULD_BE_32+_CHARS_LONG_123456";
    }
    else
    {
        throw new Exception("Jwt:Key missing in configuration.");
    }
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

// -----------------------------------------------------
//  6. BUILD + MIDDLEWARE PIPELINE
// -----------------------------------------------------
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

// -----------------------------------------------------
//  7. REQUIRED FOR INTEGRATION TESTS (WebApplicationFactory)
// -----------------------------------------------------
public partial class Program { }