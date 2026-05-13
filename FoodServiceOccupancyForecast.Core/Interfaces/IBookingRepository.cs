using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodServiceOccupancyForecast.Core.Entities;

namespace FoodServiceOccupancyForecast.Core.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetByTableIdAsync(int tableId);
        Task<IEnumerable<Booking>> GetByTableAsync(int tableId);
        Task<IEnumerable<Booking>> GetByTableAndTimeAsync(int tableId, DateTime time);
        Task<IEnumerable<Booking>> GetByDateAsync(DateTime date);
        Task<IEnumerable<Booking>> GetBookingsForDateAsync(DateTime date);
        Task<IEnumerable<Booking>> GetPendingAsync();
        Task ConfirmAsync(int id);
        Task CancelAsync(int id);
    }
}
