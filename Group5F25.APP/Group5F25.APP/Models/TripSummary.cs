namespace Group5F25.APP.Models
{
    public class TripSummary
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int DurationSeconds { get; set; }
        public int? SafetyScore { get; set; }
    }
}
