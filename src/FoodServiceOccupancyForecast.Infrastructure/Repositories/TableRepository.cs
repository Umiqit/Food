using Microsoft.EntityFrameworkCore;
using FoodServiceOccupancyForecast.Core.Entities;
using FoodServiceOccupancyForecast.Core.Enums;
using FoodServiceOccupancyForecast.Core.Interfaces;
using FoodServiceOccupancyForecast.Infrastructure.Data;

namespace FoodServiceOccupancyForecast.Infrastructure.Repositories;

public class TableRepository : ITableRepository
{
    private readonly ApplicationDbContext _context;

    public TableRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Table?> GetByIdAsync(int id) => 
        await _context.Tables.FindAsync(id);

    public async Task<IEnumerable<Table>> GetAllAsync() => 
        await _context.Tables.Include(t => t.Bookings).ToListAsync();

    public async Task<IEnumerable<Table>> GetByStatusAsync(TableStatus status) => 
        await _context.Tables.Where(t => t.Status == status).ToListAsync();

    public async Task<IEnumerable<Table>> GetAvailableAsync(DateTime date, TimeSpan start, TimeSpan end)
    {
        var bookedTableIds = await _context.Bookings
            .Where(b => b.BookingDate.Date == date.Date && b.Status == BookingStatus.Confirmed)
            .Where(b => (b.StartTime <= end && b.EndTime >= start))
            .Select(b => b.TableId)
            .ToListAsync();

        return await _context.Tables
            .Where(t => !bookedTableIds.Contains(t.Id) && t.Status != TableStatus.Occupied)
            .ToListAsync();
    }

    public async Task<IEnumerable<Table>> GetByHallAsync(string hallName) => 
        await _context.Tables.Where(t => t.HallName == hallName).ToListAsync();

    public async Task AddAsync(Table entity)
    {
        _context.Tables.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Table entity)
    {
        _context.Tables.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(int id, TableStatus status, int? guests = null)  // Made optional
    {
        var table = await _context.Tables.FindAsync(id);
        if (table != null)
        {
            table.Status = status;
            if (guests.HasValue) table.CurrentGuests = guests.Value;
            if (status == TableStatus.Occupied) 
                table.OccupiedSince = DateTime.UtcNow;
            else if (status == TableStatus.Free) 
            { 
                table.CurrentGuests = 0; 
                table.OccupiedSince = null; 
            }
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Tables.FindAsync(id);
        if (entity != null)
        {
            _context.Tables.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
