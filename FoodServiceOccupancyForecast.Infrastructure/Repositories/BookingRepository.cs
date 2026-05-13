using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FoodServiceOccupancyForecast.Core.Entities;
using FoodServiceOccupancyForecast.Core.Enums;
using FoodServiceOccupancyForecast.Core.Interfaces;
using FoodServiceOccupancyForecast.Infrastructure.Data;

namespace FoodServiceOccupancyForecast.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // === IRepository<Booking> ===
        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings.ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings.FindAsync(id);
        }

        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }

        // === IBookingRepository ===
        public async Task<IEnumerable<Booking>> GetByTableIdAsync(int tableId)
        {
            return await _context.Bookings.Where(b => b.TableId == tableId).ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByTableAsync(int tableId)
        {
            return await _context.Bookings.Where(b => b.TableId == tableId).ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByTableAndTimeAsync(int tableId, DateTime time)
        {
            return await _context.Bookings
                .Where(b => b.TableId == tableId
                         && b.BookingTime.Date == time.Date
                         && b.BookingTime.Hour == time.Hour)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByDateAsync(DateTime date)
        {
            return await _context.Bookings
                .Where(b => b.BookingTime.Date == date.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsForDateAsync(DateTime date)
        {
            return await _context.Bookings
                .Where(b => b.BookingTime.Date == date.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetPendingAsync()
        {
            return await _context.Bookings
                .Where(b => b.Status == BookingStatus.Pending)
                .ToListAsync();
        }

        public async Task ConfirmAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.Status = BookingStatus.Confirmed;
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
    }
}
