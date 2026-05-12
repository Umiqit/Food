using Microsoft.EntityFrameworkCore;
using FoodServiceOccupancyForecast.Core.Entities;
using FoodServiceOccupancyForecast.Core.Enums;
using FoodServiceOccupancyForecast.Core.Interfaces;
using FoodServiceOccupancyForecast.Infrastructure.Data;

namespace FoodServiceOccupancyForecast.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _context;

    public BookingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Booking?> GetByIdAsync(int id) => 
        await _context.Bookings.Include(b => b.Table).FirstOrDefaultAsync(b => b.Id == id);

    public async Task<IEnumerable<Booking>> GetAllAsync() => 
        await _context.Bookings.Include(b => b.Table).OrderByDescending(b => b.CreatedAt).ToListAsync();

    public async Task<IEnumerable<Booking>> GetPendingAsync() => 
        await _context.Bookings.Include(b => b.Table).Where(b => b.Status == BookingStatus.Pending).ToListAsync();

    public async Task<IEnumerable<Booking>> GetByDateAsync(DateTime date) => 
        await _context.Bookings.Include(b => b.Table).Where(b => b.BookingDate.Date == date.Date).ToListAsync();

    public async Task<IEnumerable<Booking>> GetByTableAsync(int tableId) => 
        await _context.Bookings.Where(b => b.TableId == tableId).ToListAsync();

    public async Task<IEnumerable<Booking>> GetByTableAndTimeAsync(int tableId, DateTime dateTime)
    {
        return await _context.Bookings
            .Where(b => b.TableId == tableId 
                && b.BookingDate.Date == dateTime.Date
                && b.StartTime <= dateTime.TimeOfDay 
                && b.EndTime >= dateTime.TimeOfDay)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetBookingsForDateAsync(DateTime date)
    {
        return await _context.Bookings
            .Include(b => b.Table)
            .Where(b => b.BookingDate.Date == date.Date)
            .OrderBy(b => b.StartTime)
            .ToListAsync();
    }

    public async Task AddAsync(Booking entity)
    {
        _context.Bookings.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Booking entity)
    {
        _context.Bookings.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task ConfirmAsync(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null)
        {
            booking.Status = BookingStatus.Confirmed;
            booking.ConfirmedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task CancelAsync(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null)
        {
            booking.Status = BookingStatus.Cancelled;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Bookings.FindAsync(id);
        if (entity != null)
        {
            _context.Bookings.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
