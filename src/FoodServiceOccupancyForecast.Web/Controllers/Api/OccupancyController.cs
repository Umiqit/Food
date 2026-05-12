using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using FoodServiceOccupancyForecast.Core.Interfaces;
using FoodServiceOccupancyForecast.Core.Entities;
using FoodServiceOccupancyForecast.Web.Hubs;

namespace FoodServiceOccupancyForecast.Web.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class OccupancyController : ControllerBase
{
    private readonly IOccupancyService _occupancyService;
    private readonly IHubContext<OccupancyHub> _hubContext;

    public OccupancyController(IOccupancyService occupancyService, IHubContext<OccupancyHub> hubContext)
    {
        _occupancyService = occupancyService;
        _hubContext = hubContext;
    }

    [HttpGet("current")]
    public async Task<ActionResult<OccupancySnapshot>> GetCurrent()
    {
        var snapshot = await _occupancyService.GetCurrentAsync();
        return Ok(snapshot);
    }

    [HttpGet("peak")]
    public async Task<ActionResult<IEnumerable<OccupancySnapshot>>> GetPeak([FromQuery] DateTime? date)
    {
        var targetDate = date ?? DateTime.UtcNow;
        var peaks = await _occupancyService.GetPeakHoursAsync(targetDate);
        return Ok(peaks);
    }

    [HttpGet("forecast")]
    public async Task<ActionResult<IEnumerable<OccupancySnapshot>>> GetForecast(
        [FromQuery] DateTime? date, [FromQuery] int days = 7)
    {
        var targetDate = date ?? DateTime.UtcNow;
        var forecast = await _occupancyService.GetForecastAsync(targetDate, days);
        return Ok(forecast);
    }

    [HttpGet("load")]
    public async Task<ActionResult<double>> GetLoadPercentage()
    {
        var load = await _occupancyService.GetLoadPercentageAsync();
        return Ok(load);
    }
}
