using Microsoft.AspNetCore.SignalR;

namespace Dream_Bridge.Services.Notifications
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string userId, string message, string type)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", message, type);
        }
    }
}
