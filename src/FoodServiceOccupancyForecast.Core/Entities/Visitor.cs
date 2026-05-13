using System;

namespace FoodServiceOccupancyForecast.Core.Entities
{
    public class Visitor
    {
        public int Id { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public int? TableId { get; set; }
        public int? CameraId { get; set; }
        public string? DetectionSource { get; set; }
    }
}
