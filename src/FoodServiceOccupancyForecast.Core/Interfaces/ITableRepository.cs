using System.Collections.Generic;
using System.Threading.Tasks;
using FoodServiceOccupancyForecast.Core.Entities;

namespace FoodServiceOccupancyForecast.Core.Interfaces
{
    public interface ITableRepository
    {
        Task<IEnumerable<Table>> GetAllAsync();
        Task<Table?> GetByIdAsync(int id);
        Task<Table?> GetByNumberAsync(int number);
        Task UpdateAsync(Table table);
        Task<IEnumerable<Table>> GetByHallIdAsync(int hallId);
        Task<IEnumerable<Table>> GetByStatusAsync(TableStatus status);
    }
}
