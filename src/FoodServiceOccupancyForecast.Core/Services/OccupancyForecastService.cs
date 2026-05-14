using FoodServiceOccupancyForecast.Core.Entities;
using FoodServiceOccupancyForecast.Core.Interfaces;

namespace FoodServiceOccupancyForecast.Core.Services;

public class OccupancyForecastService : IOccupancyService
{
    private readonly ITableRepository _tableRepo;
    private readonly IBookingRepository _bookingRepo;

    public OccupancyForecastService(ITableRepository tableRepo, IBookingRepository bookingRepo)
    {
        _tableRepo = tableRepo;
        _bookingRepo = bookingRepo;
    }

    public async Task<OccupancySnapshot> GetCurrentAsync()
    {
        var tables = await _tableRepo.GetAllAsync();
        var occupied = tables.Count(t => t.Status == Enums.TableStatus.Occupied);
        var reserved = tables.Count(t => t.Status == Enums.TableStatus.Reserved);
        var free = tables.Count(t => t.Status == Enums.TableStatus.Free);
        var total = tables.Count();
        var guests = tables.Sum(t => t.CurrentGuests ?? 0);

        return new OccupancySnapshot
        {
            TotalGuests = guests,
            OccupiedTables = occupied,
            ReservedTables = reserved,
            FreeTables = free,
            LoadPercentage = total > 0 ? (occupied + reserved) * 100.0 / total : 0,
            Timestamp = DateTime.UtcNow
        };
    }

    public async Task<IEnumerable<OccupancySnapshot>> GetPeakHoursAsync(DateTime date)
    {
        var snapshots = new List<OccupancySnapshot>();
        for (int hour = 10; hour <= 23; hour++)
        {
            var load = hour switch
            {
                >= 12 and <= 14 => 85.0,
                >= 18 and <= 21 => 95.0,
                >= 10 and <= 11 => 45.0,
                >= 15 and <= 17 => 60.0,
                _ => 30.0
            };
            snapshots.Add(new OccupancySnapshot
            {
                Timestamp = date.Date.AddHours(hour),
                LoadPercentage = load,
                PeakHour = $"{hour:00}:00"
            });
        }
        return snapshots;
    }

    public async Task<IEnumerable<OccupancySnapshot>> GetForecastAsync(DateTime date, int days)
    {
        var forecast = new List<OccupancySnapshot>();
        for (int d = 0; d < days; d++)
        {
            var dayData = await GetPeakHoursAsync(date.AddDays(d));
            forecast.AddRange(dayData);
        }
        return forecast;
    }

    public async Task<double> GetLoadPercentageAsync()
    {
        var current = await GetCurrentAsync();
        return current.LoadPercentage;
    }
}
