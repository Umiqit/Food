using FoodServiceOccupancyForecast.Core.Entities;
using FoodServiceOccupancyForecast.Core.Enums;

namespace FoodServiceOccupancyForecast.Web.Services;

public class InMemoryRestaurantStore
{
    private int _nextBookingId = 1;

    public List<Table> Tables { get; } =
    [
        new() { Id = 1, Number = 1, HallId = 1, Capacity = 4, Status = TableStatus.Reserved, CurrentGuests = 0 },
        new() { Id = 2, Number = 2, HallId = 1, Capacity = 4, Status = TableStatus.Free, CurrentGuests = 0 },
        new() { Id = 3, Number = 3, HallId = 1, Capacity = 4, Status = TableStatus.Free, CurrentGuests = 0 },
        new() { Id = 4, Number = 4, HallId = 1, Capacity = 6, Status = TableStatus.Free, CurrentGuests = 0 },
        new() { Id = 5, Number = 5, HallId = 1, Capacity = 4, Status = TableStatus.Free, CurrentGuests = 0 },
        new() { Id = 6, Number = 6, HallId = 1, Capacity = 4, Status = TableStatus.Occupied, CurrentGuests = 3 },
        new() { Id = 7, Number = 7, HallId = 1, Capacity = 4, Status = TableStatus.Free, CurrentGuests = 0 },
        new() { Id = 8, Number = 8, HallId = 1, Capacity = 6, Status = TableStatus.Reserved, CurrentGuests = 0 },
        new() { Id = 9, Number = 9, HallId = 1, Capacity = 4, Status = TableStatus.Occupied, CurrentGuests = 4 },
        new() { Id = 10, Number = 10, HallId = 1, Capacity = 6, Status = TableStatus.Free, CurrentGuests = 0 },
        new() { Id = 11, Number = 11, HallId = 1, Capacity = 6, Status = TableStatus.Occupied, CurrentGuests = 5 },
        new() { Id = 13, Number = 13, HallId = 2, Capacity = 4, Status = TableStatus.Free, CurrentGuests = 0 },
        new() { Id = 14, Number = 14, HallId = 2, Capacity = 4, Status = TableStatus.Free, CurrentGuests = 0 },
        new() { Id = 15, Number = 15, HallId = 2, Capacity = 4, Status = TableStatus.Occupied, CurrentGuests = 2 },
        new() { Id = 16, Number = 16, HallId = 2, Capacity = 4, Status = TableStatus.Free, CurrentGuests = 0 },
        new() { Id = 17, Number = 17, HallId = 2, Capacity = 4, Status = TableStatus.Reserved, CurrentGuests = 0 },
        new() { Id = 18, Number = 18, HallId = 2, Capacity = 4, Status = TableStatus.Occupied, CurrentGuests = 4 },
        new() { Id = 19, Number = 19, HallId = 4, Capacity = 4, Status = TableStatus.Occupied, CurrentGuests = 3 },
        new() { Id = 20, Number = 20, HallId = 4, Capacity = 4, Status = TableStatus.Free, CurrentGuests = 0 },
        new() { Id = 21, Number = 21, HallId = 4, Capacity = 4, Status = TableStatus.Free, CurrentGuests = 0 },
        new() { Id = 24, Number = 24, HallId = 3, Capacity = 4, Status = TableStatus.Free, CurrentGuests = 0 },
        new() { Id = 25, Number = 25, HallId = 3, Capacity = 4, Status = TableStatus.Free, CurrentGuests = 0 },
        new() { Id = 26, Number = 26, HallId = 3, Capacity = 4, Status = TableStatus.Free, CurrentGuests = 0 },
        new() { Id = 27, Number = 27, HallId = 3, Capacity = 4, Status = TableStatus.Free, CurrentGuests = 0 },
        new() { Id = 28, Number = 28, HallId = 3, Capacity = 4, Status = TableStatus.Free, CurrentGuests = 0 },
        new() { Id = 29, Number = 29, HallId = 3, Capacity = 4, Status = TableStatus.Free, CurrentGuests = 0 }
    ];

    public List<Booking> Bookings { get; } = [];

    public int GetNextBookingId()
    {
        return _nextBookingId++;
    }
}
