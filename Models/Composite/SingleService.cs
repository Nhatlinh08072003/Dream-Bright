namespace Dream_Bridge.Models.Composite
{
    public class SingleService : IService
    {
        private readonly string _name;

        public SingleService(string name)
        {
            _name = name;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"✅ {_name}");
        }

        public string GetName() => _name; // Implement this method
    }
}