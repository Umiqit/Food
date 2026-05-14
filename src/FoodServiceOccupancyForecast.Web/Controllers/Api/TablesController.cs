using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using FoodServiceOccupancyForecast.Core.Entities;
using FoodServiceOccupancyForecast.Core.Enums;
using FoodServiceOccupancyForecast.Web.Hubs;
using FoodServiceOccupancyForecast.Web.Services;

namespace FoodServiceOccupancyForecast.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class TablesController : ControllerBase
    {
        private readonly TableService _tableService;
        private readonly IHubContext<TableHub> _hubContext;

        public TablesController(TableService tableService, IHubContext<TableHub> hubContext)
        {
            _tableService = tableService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Table>>> GetAll()
        {
            var tables = await _tableService.GetAllTablesAsync();
            return Ok(tables);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Table>> GetById(int id)
        {
            var table = await _tableService.GetTableByIdAsync(id);
            if (table == null) return NotFound();
            return Ok(table);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            await _tableService.UpdateTableStatusAsync(id, request.Status);
            await _hubContext.Clients.All.SendAsync("TableStatusChanged", id, request.Status.ToString());
            return NoContent();
        }

        [HttpGet("statistics")]
        public async Task<ActionResult<TableStatistics>> GetStatistics()
        {
            var total = (await _tableService.GetAllTablesAsync()).Count();
            var occupied = await _tableService.GetOccupiedCountAsync();
            var reserved = await _tableService.GetReservedCountAsync();
            var guests = await _tableService.GetTotalGuestsAsync();

            return Ok(new TableStatistics
            {
                Total = total,
                Occupied = occupied,
                Reserved = reserved,
                Free = total - occupied - reserved,
                Guests = guests,
                LoadPercentage = total > 0 ? (occupied * 100 / total) : 0
            });
        }
    }

    public class UpdateStatusRequest
    {
        public TableStatus Status { get; set; }
    }

    public class TableStatistics
    {
        public int Total { get; set; }
        public int Occupied { get; set; }
        public int Reserved { get; set; }
        public int Free { get; set; }
        public int Guests { get; set; }
        public int LoadPercentage { get; set; }
    }
}
