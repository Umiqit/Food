using FoodServiceOccupancyForecast.Core.Entities;
using FoodServiceOccupancyForecast.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodServiceOccupancyForecast.Infrastructure.Repositories;

public interface IOccupancyRepository : IRepository<OccupancyRecord>
{
    Task<List<OccupancyRecord>> GetRecordsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<OccupancyRecord?> GetLatestRecordAsync();
    Task<List<OccupancyRecord>> GetPeakHoursAsync();
}

public class OccupancyRepository : Repository<OccupancyRecord>, IOccupancyRepository
{
    public OccupancyRepository(ApplicationDbContext context) : base(context) { }

    public async Task<List<OccupancyRecord>> GetRecordsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(r => r.Timestamp >= startDate && r.Timestamp <= endDate)
            .OrderByDescending(r => r.Timestamp)
            .ToListAsync();
    }

    public async Task<OccupancyRecord?> GetLatestRecordAsync()
    {
        return await _dbSet
            .OrderByDescending(r => r.Timestamp)
            .FirstOrDefaultAsync();
    }

    public async Task<List<OccupancyRecord>> GetPeakHoursAsync()
    {
        var lastDays = DateTime.UtcNow.AddDays(-7);
        return await _dbSet
            .Where(r => r.Timestamp >= lastDays)
            .OrderByDescending(r => r.OccupancyPercent)
            .Take(10)
            .ToListAsync();
    }
}
