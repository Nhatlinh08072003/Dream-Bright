namespace Dream_Bridge.Models.Observer
{
    public class EventNotifier
    {
        private readonly List<IEventObserver> _observers = new();

        public void Attach(IEventObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IEventObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Update(message);
            }
        }
    }
}
