using System;

namespace Dream_Bridge.Models.Observer
{
    public class AdminObserver : IObserver
    {
        public void Update(string message)
        {
            Console.WriteLine($"🔔 Admin nhận thông báo: {message}");
        }
    }
}
