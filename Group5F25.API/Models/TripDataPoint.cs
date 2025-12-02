using Group_5_Project_Ayman_Birendra_Cole_Rasik.API.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Group5F25.API.Models
{
    public class TripDataPoint
    {
        [Key]
        public int Id { get; set; }

        public int TripId { get; set; } // Foreign Key

        public DateTime Timestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Speed { get; set; }
        public float AccelerationX { get; set; }
        public float AccelerationY { get; set; }
        public float AccelerationZ { get; set; }

        [JsonIgnore]
        public Trip? Trip { get; set; }
    }
}