using System.Collections.Generic;
using System.Threading.Tasks;
using FoodServiceOccupancyForecast.Core.Entities;
using FoodServiceOccupancyForecast.Core.Enums;
using FoodServiceOccupancyForecast.Core.Interfaces;

namespace FoodServiceOccupancyForecast.Core.Services
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

        public async Task UpdateTableStatusAsync(int tableId, TableStatus status)
        {
            var table = await _tableRepository.GetByIdAsync(tableId);
            if (table != null)
            {
                table.Status = status;
                await _tableRepository.UpdateAsync(table);
            }
        }

        public async Task<int> GetOccupiedCountAsync()
        {
            var tables = await _tableRepository.GetAllAsync();
            int count = 0;
            foreach (var t in tables) if (t.Status == TableStatus.Occupied) count++;
            return count;
        }

        public async Task<int> GetReservedCountAsync()
        {
            var tables = await _tableRepository.GetAllAsync();
            int count = 0;
            foreach (var t in tables) if (t.Status == TableStatus.Reserved) count++;
            return count;
        }

        public async Task<int> GetTotalGuestsAsync()
        {
            var tables = await _tableRepository.GetAllAsync();
            int guests = 0;
            foreach (var t in tables)
                if (t.Status == TableStatus.Occupied && t.CurrentGuests.HasValue)
                    guests += t.CurrentGuests.Value;
            return guests;
        }
    }
}
