using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using FoodServiceOccupancyForecast.Core.Entities;
using FoodServiceOccupancyForecast.Core.Interfaces;
using FoodServiceOccupancyForecast.Web.Hubs;

namespace FoodServiceOccupancyForecast.Web.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ITableRepository _tableRepository;
    private readonly IHubContext<OccupancyHub> _hubContext;

    public BookingsController(
        IBookingRepository bookingRepository, 
        ITableRepository tableRepository,
        IHubContext<OccupancyHub> hubContext)
    {
        _bookingRepository = bookingRepository;
        _tableRepository = tableRepository;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Booking>>> GetAll()
    {
        var bookings = await _bookingRepository.GetAllAsync();
        return Ok(bookings);
    }

    [HttpGet("pending")]
    public async Task<ActionResult<IEnumerable<Booking>>> GetPending()
    {
        var bookings = await _bookingRepository.GetPendingAsync();
        return Ok(bookings);
    }

    [HttpGet("by-date/{date:datetime}")]
    public async Task<ActionResult<IEnumerable<Booking>>> GetByDate(DateTime date)
    {
        var bookings = await _bookingRepository.GetByDateAsync(date);
        return Ok(bookings);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Booking>> GetById(int id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);
        if (booking == null) return NotFound();
        return Ok(booking);
    }

    [HttpPost]
    public async Task<ActionResult<Booking>> Create([FromBody] CreateBookingRequest request)
    {
        var booking = new Booking
        {
            TableId = request.TableId,
            CustomerName = request.CustomerName,
            CustomerPhone = request.CustomerPhone,
            GuestsCount = request.GuestsCount,
            BookingDate = request.BookingDate,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Notes = request.Notes
        };

        await _bookingRepository.AddAsync(booking);

        await _tableRepository.UpdateStatusAsync(request.TableId, Core.Enums.TableStatus.Reserved);

        await OccupancyHub.BroadcastNewBooking(_hubContext, new { 
            Id = booking.Id, 
            TableId = booking.TableId, 
            CustomerName = booking.CustomerName,
            Status = booking.Status.ToString()
        });

        return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
    }

    [HttpPut("{id}/confirm")]
    public async Task<IActionResult> Confirm(int id)
    {
        await _bookingRepository.ConfirmAsync(id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Cancel(int id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);
        if (booking == null) return NotFound();

        await _bookingRepository.CancelAsync(id);

        await _tableRepository.UpdateStatusAsync(booking.TableId, Core.Enums.TableStatus.Free);

        return NoContent();
    }
}

public class CreateBookingRequest
{
    public int TableId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? CustomerPhone { get; set; }
    public string? CustomerEmail { get; set; }
    public int GuestsCount { get; set; }
    public DateTime BookingDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string? Notes { get; set; }
}
