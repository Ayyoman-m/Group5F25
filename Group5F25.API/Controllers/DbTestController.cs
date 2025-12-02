using Group5F25.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Group5F25.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DbTestController : ControllerBase
    {
        private readonly AppDbContext _db;

        public DbTestController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/DbTest/ping
        [HttpGet("ping")]
        public async Task<IActionResult> Ping()
        {
            try
            {
                var count = await _db.Users.CountAsync();
                return Ok(new { message = "Database connection successful", userCount = count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Database connection failed", error = ex.Message });
            }
        }
    }
}
