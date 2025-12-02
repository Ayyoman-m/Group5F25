using Group5F25.API.Data;                                     // AppDbContext
using Group5F25.API.DTOs;                                     // TripUploadRequest
using Group_5_Project_Ayman_Birendra_Cole_Rasik.API.Models;   // Trip
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group5F25.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripController : ControllerBase
    {
        private readonly AppDbContext _db;

        public TripController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadTrip([FromBody] TripUploadRequest dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var trip = new Trip
            {
                UserId = dto.UserId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                DurationSeconds = dto.DurationSeconds,
                SafetyScore = dto.SafetyScore,
                Notes = dto.Notes
            };

            _db.Trips.Add(trip);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                trip.Id,
                trip.DurationSeconds,
                trip.SafetyScore,
                trip.StartTime,
                trip.EndTime
            });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTripsForUser(int userId)
        {
            var trips = await _db.Trips
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.StartTime)
                .ToListAsync();

            return Ok(trips);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTripById(int id)
        {
            var trip = await _db.Trips.FindAsync(id);
            if (trip == null)
                return NotFound();

            return Ok(trip);
        }
    }
}
