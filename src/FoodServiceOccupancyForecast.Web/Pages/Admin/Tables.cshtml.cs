using Microsoft.AspNetCore.Mvc.RazorPages;
using FoodServiceOccupancyForecast.Core.Interfaces;
using FoodServiceOccupancyForecast.Core.Entities;

namespace FoodServiceOccupancyForecast.Web.Pages.Admin;

public class TablesModel : PageModel
{
    private readonly ITableRepository _tableRepository;

    public List<Table> Tables { get; set; } = new();

    public TablesModel(ITableRepository tableRepository)
    {
        _tableRepository = tableRepository;
    }

    public async Task OnGetAsync()
    {
        Tables = (await _tableRepository.GetAllAsync()).ToList();
    }
}
