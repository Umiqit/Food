using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodServiceOccupancyForecast.Core.Entities;
using FoodServiceOccupancyForecast.Core.Enums;

namespace FoodServiceOccupancyForecast.Core.Interfaces
{
    public interface ITableRepository : IRepository<Table>
    {
        Task<Table?> GetByNumberAsync(int number);
        Task<IEnumerable<Table>> GetByHallIdAsync(int hallId);
        Task<IEnumerable<Table>> GetByHallAsync(string hallName);
        Task<IEnumerable<Table>> GetByStatusAsync(TableStatus status);
        Task<IEnumerable<Table>> GetAvailableAsync(DateTime date, TimeSpan startTime, TimeSpan endTime);
        Task UpdateStatusAsync(int id, TableStatus status, int? currentGuests);
    }
}
