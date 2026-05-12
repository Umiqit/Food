using Microsoft.AspNetCore.SignalR;
using FoodServiceOccupancyForecast.Core.Entities;

namespace FoodServiceOccupancyForecast.Web.Hubs;

public class OccupancyHub : Hub
{
    public async Task SubscribeToUpdates(string zone)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, zone);
    }

    public async Task UnsubscribeFromUpdates(string zone)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, zone);
    }

    public static async Task BroadcastOccupancyUpdate(IHubContext<OccupancyHub> hubContext, OccupancySnapshot snapshot)
    {
        await hubContext.Clients.All.SendAsync("OccupancyUpdated", snapshot);
    }

    public static async Task BroadcastTableStatusChange(IHubContext<OccupancyHub> hubContext, int tableId, string status, int guests)
    {
        await hubContext.Clients.All.SendAsync("TableStatusChanged", new { TableId = tableId, Status = status, Guests = guests });
    }

    public static async Task BroadcastNewBooking(IHubContext<OccupancyHub> hubContext, object booking)
    {
        await hubContext.Clients.All.SendAsync("NewBooking", booking);
    }
}
