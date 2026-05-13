using FoodServiceOccupancyForecast.Core.Enums;

namespace FoodServiceOccupancyForecast.Core.Entities
{
    public class Table
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int HallId { get; set; }
        public int Capacity { get; set; }
        public TableStatus Status { get; set; }
        public int? CurrentGuests { get; set; }
        public int? WaiterId { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Rotation { get; set; }
        public string? Shape { get; set; }
    }
}
