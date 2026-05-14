using FoodServiceOccupancyForecast.Core.Entities;
using FoodServiceOccupancyForecast.Core.Enums;
using FoodServiceOccupancyForecast.Core.Interfaces;

namespace FoodServiceOccupancyForecast.Web.Services;

public class InMemoryTableRepository : ITableRepository
{
    private readonly InMemoryRestaurantStore _store;

    public InMemoryTableRepository(InMemoryRestaurantStore store)
    {
        _store = store;
    }

    public Task<IEnumerable<Table>> GetAllAsync()
    {
        return Task.FromResult(_store.Tables.AsEnumerable());
    }

    public Task<Table?> GetByIdAsync(int id)
    {
        return Task.FromResult(_store.Tables.FirstOrDefault(t => t.Id == id));
    }

    public Task AddAsync(Table table)
    {
        _store.Tables.Add(table);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Table table)
    {
        var index = _store.Tables.FindIndex(t => t.Id == table.Id);
        if (index >= 0)
        {
            _store.Tables[index] = table;
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        _store.Tables.RemoveAll(t => t.Id == id);
        return Task.CompletedTask;
    }

    public Task<Table?> GetByNumberAsync(int number)
    {
        return Task.FromResult(_store.Tables.FirstOrDefault(t => t.Number == number));
    }

    public Task<IEnumerable<Table>> GetByHallIdAsync(int hallId)
    {
        return Task.FromResult(_store.Tables.Where(t => t.HallId == hallId));
    }

    public Task<IEnumerable<Table>> GetByHallAsync(string hallName)
    {
        var hallId = hallName switch
        {
            "Main hall" => 1,
            "Circle Hall" => 2,
            "2nd floor" => 3,
            "Veranda" => 4,
            _ => 0
        };

        return Task.FromResult(_store.Tables.Where(t => t.HallId == hallId));
    }

    public Task<IEnumerable<Table>> GetByStatusAsync(TableStatus status)
    {
        return Task.FromResult(_store.Tables.Where(t => t.Status == status));
    }

    public Task<IEnumerable<Table>> GetAvailableAsync(DateTime date, TimeSpan startTime, TimeSpan endTime)
    {
        var start = date.Date + startTime;
        var end = date.Date + endTime;
        var bookedTableIds = _store.Bookings
            .Where(b => b.BookingTime >= start && b.BookingTime < end && b.Status != BookingStatus.Cancelled)
            .Select(b => b.TableId)
            .ToHashSet();

        var tables = _store.Tables
            .Where(t => !bookedTableIds.Contains(t.Id)
                && t.Status != TableStatus.Unavailable
                && t.Status != TableStatus.Cleaning);

        return Task.FromResult(tables);
    }

    public Task UpdateStatusAsync(int id, TableStatus status, int? currentGuests)
    {
        var table = _store.Tables.FirstOrDefault(t => t.Id == id);
        if (table != null)
        {
            table.Status = status;
            table.CurrentGuests = currentGuests;
        }

        return Task.CompletedTask;
    }
}
