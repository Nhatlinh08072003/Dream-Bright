using System.Collections.Generic;

namespace Dream_Bridge.Models.Observer
{
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify(string message);
    }
}
