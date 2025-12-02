namespace Group5F25.APP.Models
{
    public class TripUploadRequest
    {
        public int UserId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int DurationSeconds { get; set; }

        public int? SafetyScore { get; set; }
        public string? Notes { get; set; }
        public List<TripDataPoint> DataPoints { get; set; } = new();
    }
}