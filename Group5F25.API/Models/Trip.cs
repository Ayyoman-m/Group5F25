using System.ComponentModel.DataAnnotations;

namespace Group5F25.API.Models
{
    public class Trip
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int DurationSeconds { get; set; }
        public int? SafetyScore { get; set; }
        public string? Notes { get; set; }

        // 👇 NEW: Navigation property
        public List<TripDataPoint> DataPoints { get; set; } = new();
    }
}