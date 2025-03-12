namespace Dream_Bridge.Models.Observer
{
    public class AdminObserver : IEventObserver
    {
        public void Update(string eventMessage)
        {
            // Xử lý thông báo cho admin
            Console.WriteLine($"Admin received: {eventMessage}");
        }
    }
}
