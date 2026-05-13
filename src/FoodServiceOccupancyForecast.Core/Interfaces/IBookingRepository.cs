using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodServiceOccupancyForecast.Core.Entities;

namespace FoodServiceOccupancyForecast.Core.Interfaces
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAllAsync();
        Task<Booking?> GetByIdAsync(int id);
        Task<IEnumerable<Booking>> GetByTableIdAsync(int tableId);
        Task<IEnumerable<Booking>> GetByDateAsync(DateTime date);
        Task<IEnumerable<Booking>> GetPendingAsync();
        Task AddAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task DeleteAsync(int id);
    }
}
