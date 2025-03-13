namespace Dream_Bridge.Services.Notifications
{
    public class UserNotificationObserver : INotificationObserver
    {
        private readonly string _userId;
        
        public UserNotificationObserver(string userId)
        {
            _userId = userId;
        }

        public string UserId => _userId;

        public void Update(string message, string type)
        {
            // Có thể lưu thông báo vào database ở đây
            Console.WriteLine($"User {_userId} received notification: {message}");
        }
    }
}
