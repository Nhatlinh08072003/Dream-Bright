using System;

namespace Dream_Bridge.Models.Observer
{
    public class UserObserver : IObserver
    {
        private readonly string _userName;
        
        public UserObserver(string userName)
        {
            _userName = userName;
        }

        public void Update(string message)
        {
            Console.WriteLine($"🔔 {_userName} nhận thông báo: {message}");
        }
    }
}
