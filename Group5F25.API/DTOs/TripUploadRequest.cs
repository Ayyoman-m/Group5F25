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
    }
}
