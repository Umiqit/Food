namespace FoodServiceOccupancyForecast.Core.Entities;

public class Visitor
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public int Count { get; set; }
    public string? Zone { get; set; }
    public string? Source { get; set; }
}
