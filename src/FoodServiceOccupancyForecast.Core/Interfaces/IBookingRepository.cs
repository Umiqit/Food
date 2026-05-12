using FoodServiceOccupancyForecast.Core.Entities;
using FoodServiceOccupancyForecast.Core.Enums;

namespace FoodServiceOccupancyForecast.Core.Interfaces;

public interface IBookingRepository : IRepository<Booking>
{
    Task<IEnumerable<Booking>> GetPendingAsync();
    Task<IEnumerable<Booking>> GetByDateAsync(DateTime date);
    Task<IEnumerable<Booking>> GetByTableAsync(int tableId);
    Task<IEnumerable<Booking>> GetByTableAndTimeAsync(int tableId, DateTime dateTime);
    Task<IEnumerable<Booking>> GetBookingsForDateAsync(DateTime date);
    Task ConfirmAsync(int id);
    Task CancelAsync(int id);
}
