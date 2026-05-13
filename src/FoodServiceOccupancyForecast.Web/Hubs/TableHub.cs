using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace FoodServiceOccupancyForecast.Web.Hubs
{
    public class TableHub : Hub
    {
        public async Task UpdateTableStatus(int tableId, string status)
        {
            await Clients.All.SendAsync("TableStatusChanged", tableId, status);
        }

        public async Task NotifyNewBooking(int tableId, string customerName)
        {
            await Clients.All.SendAsync("NewBookingReceived", tableId, customerName);
        }

        public async Task SendStaffAlert(string message)
        {
            await Clients.All.SendAsync("StaffAlert", message);
        }
    }
}
