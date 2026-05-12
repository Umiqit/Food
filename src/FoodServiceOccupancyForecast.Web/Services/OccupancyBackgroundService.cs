using FoodServiceOccupancyForecast.Core.Interfaces;
using FoodServiceOccupancyForecast.VideoAnalysis.Processing;
using FoodServiceOccupancyForecast.Web.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace FoodServiceOccupancyForecast.Web.Services;

public class OccupancyBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OccupancyBackgroundService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(10);

    public OccupancyBackgroundService(IServiceProvider serviceProvider, ILogger<OccupancyBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Occupancy background service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var occupancyService = scope.ServiceProvider.GetRequiredService<IOccupancyService>();
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<OccupancyHub>>();
                var videoService = scope.ServiceProvider.GetRequiredService<VideoProcessingService>();

                // Get current occupancy
                var snapshot = await occupancyService.GetCurrentAsync();

                // Simulate video analysis data (in real project - actual camera data)
                var zoneCounts = videoService.GetAllZonesCount();
                var totalFromCameras = zoneCounts.Values.Sum();

                // Merge camera data with snapshot
                snapshot.TotalGuests = Math.Max(snapshot.TotalGuests, totalFromCameras);

                // Broadcast to all clients
                await OccupancyHub.BroadcastOccupancyUpdate(hubContext, snapshot);

                _logger.LogInformation("Occupancy updated: {Load}% load, {Guests} guests", 
                    snapshot.LoadPercentage, snapshot.TotalGuests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in occupancy background service");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}
