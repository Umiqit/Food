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
    public class TableRepository : ITableRepository
    {
        private readonly ApplicationDbContext _context;

        public TableRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // === IRepository<Table> ===
        public async Task<IEnumerable<Table>> GetAllAsync()
        {
            return await _context.Tables.ToListAsync();
        }

        public async Task<Table?> GetByIdAsync(int id)
        {
            return await _context.Tables.FindAsync(id);
        }

        public async Task AddAsync(Table table)
        {
            await _context.Tables.AddAsync(table);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Table table)
        {
            _context.Tables.Update(table);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var table = await _context.Tables.FindAsync(id);
            if (table != null)
            {
                _context.Tables.Remove(table);
                await _context.SaveChangesAsync();
            }
        }

        // === ITableRepository ===
        public async Task<Table?> GetByNumberAsync(int number)
        {
            return await _context.Tables.FirstOrDefaultAsync(t => t.Number == number);
        }

        public async Task<IEnumerable<Table>> GetByHallIdAsync(int hallId)
        {
            return await _context.Tables.Where(t => t.HallId == hallId).ToListAsync();
        }

        public async Task<IEnumerable<Table>> GetByHallAsync(string hallName)
        {
            return await _context.Tables
                .Where(t => _context.Halls.Any(h => h.Id == t.HallId && h.Name == hallName))
                .ToListAsync();
        }

        public async Task<IEnumerable<Table>> GetByStatusAsync(TableStatus status)
        {
            return await _context.Tables.Where(t => t.Status == status).ToListAsync();
        }

        public async Task<IEnumerable<Table>> GetAvailableAsync(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            var dateTimeStart = date.Date + startTime;
            var dateTimeEnd = date.Date + endTime;

            var bookedTableIds = await _context.Bookings
                .Where(b => b.BookingTime >= dateTimeStart && b.BookingTime < dateTimeEnd
                         && b.Status != BookingStatus.Cancelled)
                .Select(b => b.TableId)
                .Distinct()
                .ToListAsync();

            return await _context.Tables
                .Where(t => !bookedTableIds.Contains(t.Id)
                         && t.Status != TableStatus.Unavailable
                         && t.Status != TableStatus.Cleaning)
                .ToListAsync();
        }

        public async Task UpdateStatusAsync(int id, TableStatus status, int? currentGuests)
        {
            var table = await _context.Tables.FindAsync(id);
            if (table != null)
            {
                table.Status = status;
                table.CurrentGuests = currentGuests;
                await _context.SaveChangesAsync();
            }
        }
    }
}
