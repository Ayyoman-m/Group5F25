using System.ComponentModel.DataAnnotations;

namespace Group5F25.API.DTOs
{
    public class TripUploadRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public int DurationSeconds { get; set; }

        public int? SafetyScore { get; set; }

        public string? Notes { get; set; }

        // 👇 NEW: This allows the API to receive the sensor data list
        public List<TripDataPointDto> DataPoints { get; set; } = new();
    }

    // A simple DTO to match the incoming JSON structure for points
    public class TripDataPointDto
    {
        public DateTime Timestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Speed { get; set; }
        public float AccelerationX { get; set; }
        public float AccelerationY { get; set; }
        public float AccelerationZ { get; set; }
    }
}