namespace Dream_Bridge.Models.Flyweight
{
    public class University
    {
        public string Name { get; }
        public string Country { get; }

        public University(string name, string country)
        {
            Name = name;
            Country = country;
        }
    }

}
