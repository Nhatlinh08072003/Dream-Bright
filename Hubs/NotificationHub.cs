using Microsoft.AspNetCore.SignalR;

namespace Dream_Bridge.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string message, string type, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                // Gửi cho tất cả clients
                await Clients.All.SendAsync("ReceiveNotification", message, type);
            }
            else
            {
                // Gửi cho user cụ thể
                await Clients.User(userId).SendAsync("ReceiveNotification", message, type);
            }
        }
    }
}
