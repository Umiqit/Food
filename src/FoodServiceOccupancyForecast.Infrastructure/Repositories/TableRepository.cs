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

        public async Task<IEnumerable<Table>> GetAllAsync()
        {
            return await _context.Tables.ToListAsync();
        }

        public async Task<Table?> GetByIdAsync(int id)
        {
            return await _context.Tables.FindAsync(id);
        }

        public async Task<Table?> GetByNumberAsync(int number)
        {
            return await _context.Tables.FirstOrDefaultAsync(t => t.Number == number);
        }

        public async Task UpdateAsync(Table table)
        {
            _context.Tables.Update(table);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Table>> GetByHallIdAsync(int hallId)
        {
            return await _context.Tables.Where(t => t.HallId == hallId).ToListAsync();
        }

        public async Task<IEnumerable<Table>> GetByStatusAsync(TableStatus status)
        {
            return await _context.Tables.Where(t => t.Status == status).ToListAsync();
        }
    }
}
