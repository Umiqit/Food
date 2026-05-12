using Xunit;
using FoodServiceOccupancyForecast.Core.Entities;
using FoodServiceOccupancyForecast.Core.Enums;

namespace FoodServiceOccupancyForecast.UnitTests;

public class BookingTests
{
    [Fact]
    public void Booking_DefaultStatus_ShouldBePending()
    {
        var booking = new Booking
        {
            TableId = 1,
            CustomerName = "Иван",
            GuestsCount = 2,
            BookingDate = DateTime.UtcNow,
            StartTime = TimeSpan.FromHours(19),
            EndTime = TimeSpan.FromHours(21)
        };

        Assert.Equal(BookingStatus.Pending, booking.Status);
    }

    [Fact]
    public void Booking_Confirm_ShouldSetConfirmedStatus()
    {
        var booking = new Booking
        {
            TableId = 1,
            CustomerName = "Иван",
            Status = BookingStatus.Pending
        };

        booking.Status = BookingStatus.Confirmed;
        booking.ConfirmedAt = DateTime.UtcNow;

        Assert.Equal(BookingStatus.Confirmed, booking.Status);
        Assert.NotNull(booking.ConfirmedAt);
    }
}
