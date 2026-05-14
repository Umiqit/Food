using FoodServiceOccupancyForecast.Core.Entities;
using FoodServiceOccupancyForecast.Core.Enums;
using FoodServiceOccupancyForecast.Core.Interfaces;

namespace FoodServiceOccupancyForecast.Web.Services;

public class InMemoryBookingRepository : IBookingRepository
{
    private readonly InMemoryRestaurantStore _store;

    public InMemoryBookingRepository(InMemoryRestaurantStore store)
    {
        _store = store;
    }

    public Task<IEnumerable<Booking>> GetAllAsync()
    {
        return Task.FromResult(_store.Bookings.AsEnumerable());
    }

    public Task<Booking?> GetByIdAsync(int id)
    {
        return Task.FromResult(_store.Bookings.FirstOrDefault(b => b.Id == id));
    }

    public Task AddAsync(Booking booking)
    {
        booking.Id = _store.GetNextBookingId();
        _store.Bookings.Add(booking);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Booking booking)
    {
        var index = _store.Bookings.FindIndex(b => b.Id == booking.Id);
        if (index >= 0)
        {
            _store.Bookings[index] = booking;
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        _store.Bookings.RemoveAll(b => b.Id == id);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Booking>> GetByTableIdAsync(int tableId)
    {
        return Task.FromResult(_store.Bookings.Where(b => b.TableId == tableId));
    }

    public Task<IEnumerable<Booking>> GetByTableAsync(int tableId)
    {
        return GetByTableIdAsync(tableId);
    }

    public Task<IEnumerable<Booking>> GetByTableAndTimeAsync(int tableId, DateTime time)
    {
        var bookings = _store.Bookings.Where(b =>
            b.TableId == tableId &&
            b.BookingTime.Date == time.Date &&
            b.BookingTime.Hour == time.Hour);

        return Task.FromResult(bookings);
    }

    public Task<IEnumerable<Booking>> GetByDateAsync(DateTime date)
    {
        return Task.FromResult(_store.Bookings.Where(b => b.BookingTime.Date == date.Date));
    }

    public Task<IEnumerable<Booking>> GetBookingsForDateAsync(DateTime date)
    {
        return GetByDateAsync(date);
    }

    public Task<IEnumerable<Booking>> GetPendingAsync()
    {
        return Task.FromResult(_store.Bookings.Where(b => b.Status == BookingStatus.Pending));
    }

    public Task ConfirmAsync(int id)
    {
        var booking = _store.Bookings.FirstOrDefault(b => b.Id == id);
        if (booking != null)
        {
            booking.Status = BookingStatus.Confirmed;
        }

        return Task.CompletedTask;
    }

    public Task CancelAsync(int id)
    {
        var booking = _store.Bookings.FirstOrDefault(b => b.Id == id);
        if (booking != null)
        {
            booking.Status = BookingStatus.Cancelled;
        }

        return Task.CompletedTask;
    }
}
