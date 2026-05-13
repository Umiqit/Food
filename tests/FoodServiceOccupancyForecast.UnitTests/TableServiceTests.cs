using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodServiceOccupancyForecast.Core.Entities;
using FoodServiceOccupancyForecast.Core.Enums;
using FoodServiceOccupancyForecast.Core.Interfaces;
using FoodServiceOccupancyForecast.Core.Services;

namespace FoodServiceOccupancyForecast.UnitTests
{
    public class TableServiceTests
    {
        private readonly Mock<ITableRepository> _mockRepo;
        private readonly TableService _service;

        public TableServiceTests()
        {
            _mockRepo = new Mock<ITableRepository>();
            _service = new TableService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllTables_ReturnsAllTables()
        {
            var tables = new List<Table>
            {
                new Table { Id = 1, Number = 1, Status = TableStatus.Free },
                new Table { Id = 2, Number = 2, Status = TableStatus.Occupied }
            };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(tables);

            var result = await _service.GetAllTablesAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task UpdateTableStatus_ChangesStatus()
        {
            var table = new Table { Id = 1, Number = 1, Status = TableStatus.Free };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(table);

            await _service.UpdateTableStatusAsync(1, TableStatus.Occupied);

            Assert.Equal(TableStatus.Occupied, table.Status);
            _mockRepo.Verify(r => r.UpdateAsync(table), Times.Once);
        }
    }
}
