using FoodServiceOccupancyForecast.Core.Entities;

namespace FoodServiceOccupancyForecast.Core.Interfaces;

public interface IOccupancyService
{
    Task<OccupancySnapshot> GetCurrentAsync();
    Task<IEnumerable<OccupancySnapshot>> GetPeakHoursAsync(DateTime date);
    Task<IEnumerable<OccupancySnapshot>> GetForecastAsync(DateTime date, int days);
    Task<double> GetLoadPercentageAsync();
}
