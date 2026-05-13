using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FoodServiceOccupancyForecast.IntegrationTests
{
    public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetTables_ReturnsSuccess()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/tables");
            response.EnsureSuccessStatusCode();
        }
    }
}
