using Microsoft.AspNetCore.SignalR;

namespace Dream_Bridge.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
         // Hàm này sẽ được gọi từ API để gửi thông báo đến tất cả các client
    public async Task SendNotification(Notification notification)
    {
        await Clients.All.SendAsync("ReceiveNotification", notification);
    }
    }
}
