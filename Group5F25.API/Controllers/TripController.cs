using Group5F25.API.Data;
using Group5F25.API.DTOs;
using Group5F25.API.Models; // Standardized namespace
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

            // 1. Create the Trip Entity
            var trip = new Trip
            {
                UserId = dto.UserId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                DurationSeconds = dto.DurationSeconds,
                SafetyScore = dto.SafetyScore,
                Notes = dto.Notes
            };

            // 2. Map the DTO DataPoints to Entity DataPoints
            if (dto.DataPoints != null && dto.DataPoints.Any())
            {
                foreach (var p in dto.DataPoints)
                {
                    trip.DataPoints.Add(new TripDataPoint
                    {
                        Timestamp = p.Timestamp,
                        Latitude = p.Latitude,
                        Longitude = p.Longitude,
                        Speed = p.Speed,
                        AccelerationX = p.AccelerationX,
                        AccelerationY = p.AccelerationY,
                        AccelerationZ = p.AccelerationZ
                    });
                }
            }

            // 3. Save to Database (EF Core handles the relationship automatically)
            _db.Trips.Add(trip);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                trip.Id,
                trip.DurationSeconds,
                PointCount = trip.DataPoints.Count, // Confirm data was saved
                trip.SafetyScore
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
            var trip = await _db.Trips
                .Include(t => t.DataPoints) // Include points when fetching detail
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trip == null)
                return NotFound();

            return Ok(trip);
        }
    }
}