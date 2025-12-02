namespace Group5F25.APP.Models
{
    public class LeaderboardEntry
    {
        public int Rank { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public double AverageScore { get; set; }
        public int TripCount { get; set; }
    }
}
