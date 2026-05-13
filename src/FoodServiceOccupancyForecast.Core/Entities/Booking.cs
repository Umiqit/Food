using System;

namespace FoodServiceOccupancyForecast.Core.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public DateTime BookingTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int GuestCount { get; set; }
        public BookingStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Completed
    }
}
