namespace FoodServiceOccupancyForecast.Core.Entities;

public class OccupancySnapshot
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public int TotalGuests { get; set; }
    public int OccupiedTables { get; set; }
    public int ReservedTables { get; set; }
    public int FreeTables { get; set; }
    public double LoadPercentage { get; set; }
    public string? PeakHour { get; set; }
    public int TotalTables { get; set; }
    public double AverageGuestsPerTable { get; set; }
    public int TurnoverRate { get; set; }
}
