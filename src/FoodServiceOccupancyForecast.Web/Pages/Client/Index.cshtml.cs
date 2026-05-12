using Microsoft.AspNetCore.Mvc.RazorPages;
using FoodServiceOccupancyForecast.Core.Interfaces;
using FoodServiceOccupancyForecast.Core.Entities;

namespace FoodServiceOccupancyForecast.Web.Pages.Client;

public class IndexModel : PageModel
{
    private readonly ITableRepository _tableRepository;

    public List<Table> Tables { get; set; } = new();

    public IndexModel(ITableRepository tableRepository)
    {
        _tableRepository = tableRepository;
    }

    public async Task OnGetAsync()
    {
        Tables = (await _tableRepository.GetAllAsync()).ToList();
    }
}
