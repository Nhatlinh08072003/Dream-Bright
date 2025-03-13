namespace Dream_Bridge.Models.Observer
{
    public interface IEventObserver
    {
        void Update(string eventMessage);
    }
}
