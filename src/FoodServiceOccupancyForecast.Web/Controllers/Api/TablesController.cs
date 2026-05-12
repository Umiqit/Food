using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using FoodServiceOccupancyForecast.Core.Entities;
using FoodServiceOccupancyForecast.Core.Enums;
using FoodServiceOccupancyForecast.Core.Interfaces;
using FoodServiceOccupancyForecast.Web.Hubs;

namespace FoodServiceOccupancyForecast.Web.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class TablesController : ControllerBase
{
    private readonly ITableRepository _tableRepository;
    private readonly IHubContext<OccupancyHub> _hubContext;

    public TablesController(ITableRepository tableRepository, IHubContext<OccupancyHub> hubContext)
    {
        _tableRepository = tableRepository;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Table>>> GetAll()
    {
        var tables = await _tableRepository.GetAllAsync();
        return Ok(tables);
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<Table>>> GetAvailable(
        [FromQuery] DateTime date, [FromQuery] TimeSpan start, [FromQuery] TimeSpan end)
    {
        var tables = await _tableRepository.GetAvailableAsync(date, start, end);
        return Ok(tables);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Table>> GetById(int id)
    {
        var table = await _tableRepository.GetByIdAsync(id);
        if (table == null) return NotFound();
        return Ok(table);
    }

    [HttpGet("by-hall/{hallName}")]
    public async Task<ActionResult<IEnumerable<Table>>> GetByHall(string hallName)
    {
        var tables = await _tableRepository.GetByHallAsync(hallName);
        return Ok(tables);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
    {
        await _tableRepository.UpdateStatusAsync(id, request.Status, request.Guests);

        var table = await _tableRepository.GetByIdAsync(id);
        await OccupancyHub.BroadcastTableStatusChange(_hubContext, id, request.Status.ToString(), request.Guests ?? 0);

        return NoContent();
    }

    [HttpPut("{id}/position")]
    public async Task<IActionResult> UpdatePosition(int id, [FromBody] UpdatePositionRequest request)
    {
        var table = await _tableRepository.GetByIdAsync(id);
        if (table == null) return NotFound();

        table.PositionX = request.X;
        table.PositionY = request.Y;
        await _tableRepository.UpdateAsync(table);

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Table>> Create([FromBody] Table table)
    {
        await _tableRepository.AddAsync(table);
        return CreatedAtAction(nameof(GetById), new { id = table.Id }, table);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _tableRepository.DeleteAsync(id);
        return NoContent();
    }
}

public class UpdateStatusRequest
{
    public TableStatus Status { get; set; }
    public int? Guests { get; set; }
}

public class UpdatePositionRequest
{
    public double X { get; set; }
    public double Y { get; set; }
}
