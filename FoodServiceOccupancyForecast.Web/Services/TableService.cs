using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodServiceOccupancyForecast.Core.Entities;
using FoodServiceOccupancyForecast.Core.Enums;
using FoodServiceOccupancyForecast.Core.Interfaces;

namespace FoodServiceOccupancyForecast.Web.Services
{
    public class TableService
    {
        private readonly ITableRepository _tableRepository;

        public TableService(ITableRepository tableRepository)
        {
            _tableRepository = tableRepository;
        }

        public async Task<IEnumerable<Table>> GetAllTablesAsync()
        {
            return await _tableRepository.GetAllAsync();
        }

        public async Task<Table?> GetTableByIdAsync(int id)
        {
            return await _tableRepository.GetByIdAsync(id);
        }

        public async Task UpdateTableStatusAsync(int id, TableStatus status)
        {
            await _tableRepository.UpdateStatusAsync(id, status, null);
        }

        public async Task<int> GetOccupiedCountAsync()
        {
            var tables = await _tableRepository.GetByStatusAsync(TableStatus.Occupied);
            return tables.Count();
        }

        public async Task<int> GetReservedCountAsync()
        {
            var tables = await _tableRepository.GetByStatusAsync(TableStatus.Reserved);
            return tables.Count();
        }

        public async Task<int> GetTotalGuestsAsync()
        {
            var tables = await _tableRepository.GetAllAsync();
            return tables.Sum(t => t.CurrentGuests ?? 0);
        }
    }
}
