using Xunit;
using FoodServiceOccupancyForecast.Core.Entities;
using FoodServiceOccupancyForecast.Core.Enums;

namespace FoodServiceOccupancyForecast.UnitTests;

public class TableTests
{
    [Fact]
    public void Table_DefaultStatus_ShouldBeFree()
    {
        var table = new Table { Name = "Test Table", Seats = 4 };
        Assert.Equal(TableStatus.Free, table.Status);
    }

    [Fact]
    public void Table_SetStatus_Occupied_ShouldWork()
    {
        var table = new Table { Name = "Test", Seats = 2 };
        table.Status = TableStatus.Occupied;
        table.CurrentGuests = 3;

        Assert.Equal(TableStatus.Occupied, table.Status);
        Assert.Equal(3, table.CurrentGuests);
    }
}
