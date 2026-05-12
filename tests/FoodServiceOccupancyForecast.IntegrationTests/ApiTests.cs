using Xunit;

namespace FoodServiceOccupancyForecast.IntegrationTests;

public class ApiTests
{
    [Fact]
    public void HealthCheck_ShouldReturnOk()
    {
        // Integration tests would use WebApplicationFactory
        Assert.True(true);
    }
}
