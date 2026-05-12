using Microsoft.AspNetCore.Mvc.RazorPages;
using FoodServiceOccupancyForecast.Core.Interfaces;
using FoodServiceOccupancyForecast.Core.Entities;

namespace FoodServiceOccupancyForecast.Web.Pages.Admin;

public class DashboardModel : PageModel
{
    private readonly IOccupancyService _occupancyService;
    private readonly IBookingRepository _bookingRepository;

    public int OccupiedTables { get; set; }
    public int TotalGuests { get; set; }
    public int ReservedTables { get; set; }
    public double LoadPercentage { get; set; }
    public List<Booking> RecentBookings { get; set; } = new();
    public List<string> HourlyLabels { get; set; } = new();
    public List<double> HourlyData { get; set; } = new();
    public List<string> WeeklyLabels { get; set; } = new();
    public List<double> WeeklyData { get; set; } = new();

    public DashboardModel(IOccupancyService occupancyService, IBookingRepository bookingRepository)
    {
        _occupancyService = occupancyService;
        _bookingRepository = bookingRepository;
    }

    public async Task OnGetAsync()
    {
        var snapshot = await _occupancyService.GetCurrentAsync();
        OccupiedTables = snapshot.OccupiedTables;
        TotalGuests = snapshot.TotalGuests;
        ReservedTables = snapshot.ReservedTables;
        LoadPercentage = Math.Round(snapshot.LoadPercentage, 1);

        var bookings = await _bookingRepository.GetAllAsync();
        RecentBookings = bookings.OrderByDescending(b => b.CreatedAt).Take(10).ToList();

        var peaks = await _occupancyService.GetPeakHoursAsync(DateTime.UtcNow);
        HourlyLabels = peaks.Select(p => p.PeakHour ?? "").ToList();
        HourlyData = peaks.Select(p => p.LoadPercentage).ToList();

        WeeklyLabels = new List<string> { "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс" };
        WeeklyData = new List<double> { 65, 70, 75, 85, 95, 90, 80 };
    }
}
