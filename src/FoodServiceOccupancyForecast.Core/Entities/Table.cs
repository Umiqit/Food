using FoodServiceOccupancyForecast.Core.Enums;

namespace FoodServiceOccupancyForecast.Core.Entities;

public class Table
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Seats { get; set; }
    public int Capacity { get; set; }
    public TableStatus Status { get; set; } = TableStatus.Free;
    public int CurrentGuests { get; set; }
    public string? Location { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public string? Shape { get; set; }
    public int? HallId { get; set; }
    public string? HallName { get; set; }
    public DateTime? OccupiedSince { get; set; }
    public DateTime? ReservedUntil { get; set; }
    public bool IsActive { get; set; } = true;
    public List<Booking> Bookings { get; set; } = new();
}
