using Microsoft.AspNetCore.Mvc.RazorPages;
using FoodServiceOccupancyForecast.Core.Interfaces;
using FoodServiceOccupancyForecast.Core.Entities;
using FoodServiceOccupancyForecast.Core.Enums;

namespace FoodServiceOccupancyForecast.Web.Pages.Admin;

public class BookingsModel : PageModel
{
    private readonly IBookingRepository _bookingRepository;

    public List<Booking> Bookings { get; set; } = new();
    public int PendingCount { get; set; }
    public int ConfirmedCount { get; set; }
    public int TodayCount { get; set; }

    public BookingsModel(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task OnGetAsync()
    {
        Bookings = (await _bookingRepository.GetAllAsync()).ToList();
        PendingCount = Bookings.Count(b => b.Status == BookingStatus.Pending);
        ConfirmedCount = Bookings.Count(b => b.Status == BookingStatus.Confirmed);
        TodayCount = Bookings.Count(b => b.BookingDate.Date == DateTime.UtcNow.Date);
    }
}
