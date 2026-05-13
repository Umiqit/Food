using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FoodServiceOccupancyForecast.Core.Entities;
using FoodServiceOccupancyForecast.Core.Services;

namespace FoodServiceOccupancyForecast.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly BookingService _bookingService;

        public BookingsController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetAll()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }

        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetPending()
        {
            var bookings = await _bookingService.GetPendingBookingsAsync();
            return Ok(bookings);
        }

        [HttpPost]
        public async Task<ActionResult<Booking>> Create([FromBody] Booking booking)
        {
            var created = await _bookingService.CreateBookingAsync(booking);
            return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
        }

        [HttpPut("{id}/confirm")]
        public async Task<IActionResult> Confirm(int id)
        {
            await _bookingService.ConfirmBookingAsync(id);
            return NoContent();
        }
    }
}
