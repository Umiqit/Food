using FoodServiceOccupancyForecast.Core.Entities;
using FoodServiceOccupancyForecast.Core.Enums;

namespace FoodServiceOccupancyForecast.Core.Interfaces;

public interface ITableRepository : IRepository<Table>
{
    Task<IEnumerable<Table>> GetByStatusAsync(TableStatus status);
    Task<IEnumerable<Table>> GetAvailableAsync(DateTime date, TimeSpan start, TimeSpan end);
    Task<IEnumerable<Table>> GetByHallAsync(string hallName);
    Task UpdateStatusAsync(int id, TableStatus status, int? guests = null);
}
