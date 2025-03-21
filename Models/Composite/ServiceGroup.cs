namespace Dream_Bridge.Models.Composite
{
    public class ServiceGroup : IService
    {
        private readonly string _name;
        private readonly List<IService> _services = new();

        public ServiceGroup(string name)
        {
            _name = name;
        }

        public string Name => _name; // Add this property

        public void AddService(IService service)
        {
            _services.Add(service);
        }

        public List<IService> GetServices() => _services;

        public void ShowInfo()
        {
            Console.WriteLine($"📌 {_name}");
            foreach (var service in _services)
            {
                service.ShowInfo();
            }
        }

        // Implement the GetName() method
        public string GetName() => _name;
    }
}