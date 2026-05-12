using Microsoft.AspNetCore.Mvc;
using FoodServiceOccupancyForecast.Core.Interfaces;

namespace FoodServiceOccupancyForecast.Web.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IOccupancyService _occupancyService;

    public AnalyticsController(IOccupancyService occupancyService)
    {
        _occupancyService = occupancyService;
    }

    [HttpGet("daily")]
    public async Task<ActionResult<object>> GetDaily([FromQuery] DateTime? date)
    {
        var targetDate = date ?? DateTime.UtcNow;
        var data = await _occupancyService.GetPeakHoursAsync(targetDate);

        return Ok(new {
            Date = targetDate,
            HourlyData = data.Select(d => new { d.Timestamp.Hour, d.LoadPercentage }),
            AverageLoad = data.Average(d => d.LoadPercentage),
            PeakHour = data.OrderByDescending(d => d.LoadPercentage).FirstOrDefault()?.PeakHour
        });
    }

    [HttpGet("weekly")]
    public async Task<ActionResult<object>> GetWeekly([FromQuery] DateTime? startDate)
    {
        var start = startDate ?? DateTime.UtcNow.AddDays(-7);
        var dailyData = new List<object>();

        for (int i = 0; i < 7; i++)
        {
            var day = start.AddDays(i);
            var dayData = await _occupancyService.GetPeakHoursAsync(day);
            dailyData.Add(new {
                Day = day.DayOfWeek.ToString(),
                Date = day.ToString("yyyy-MM-dd"),
                AverageLoad = dayData.Average(d => d.LoadPercentage),
                PeakHour = dayData.OrderByDescending(d => d.LoadPercentage).FirstOrDefault()?.PeakHour
            });
        }

        return Ok(new { StartDate = start, DailyData = dailyData });
    }

    [HttpGet("monthly")]
    public async Task<ActionResult<object>> GetMonthly([FromQuery] int? year, [FromQuery] int? month)
    {
        var targetYear = year ?? DateTime.UtcNow.Year;
        var targetMonth = month ?? DateTime.UtcNow.Month;
        var daysInMonth = DateTime.DaysInMonth(targetYear, targetMonth);
        var weeklyData = new List<object>();

        for (int week = 1; week <= 4; week++)
        {
            var weekStart = new DateTime(targetYear, targetMonth, 1).AddDays((week - 1) * 7);
            var weekData = await _occupancyService.GetPeakHoursAsync(weekStart);
            weeklyData.Add(new {
                Week = week,
                AverageLoad = weekData.Average(d => d.LoadPercentage)
            });
        }

        return Ok(new { Year = targetYear, Month = targetMonth, WeeklyData = weeklyData });
    }
}
