namespace Dream_Bridge.Services.Notifications
{
    public interface INotificationObserver
    {
        string UserId { get; }
        void Update(string message, string type);
    }
}
