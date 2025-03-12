using Microsoft.AspNetCore.SignalR;

namespace Dream_Bridge.Services.Notifications
{
    public class NotificationService
    {
        private readonly List<INotificationObserver> _observers = new();
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void RegisterObserver(INotificationObserver observer)
        {
            if (!_observers.Any(o => o.UserId == observer.UserId))
            {
                _observers.Add(observer);
            }
        }

        public void RemoveObserver(string userId)
        {
            var observer = _observers.FirstOrDefault(o => o.UserId == userId);
            if (observer != null)
            {
                _observers.Remove(observer);
            }
        }

        public async Task NotifyAsync(string message, string type, string? specificUserId = null)
        {
            var observersToNotify = specificUserId != null 
                ? _observers.Where(o => o.UserId == specificUserId)
                : _observers;

            foreach (var observer in observersToNotify)
            {
                observer.Update(message, type);
                await _hubContext.Clients.User(observer.UserId)
                    .SendAsync("ReceiveNotification", message, type);
            }
        }
    }
}
