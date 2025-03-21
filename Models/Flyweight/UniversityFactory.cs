namespace Dream_Bridge.Models.Flyweight
{
    public class UniversityFactory
    {
        private static readonly Dictionary<string, University> _universities = new();

        public static University GetUniversity(string name, string country)
        {
            string key = $"{name}-{country}";

            if (!_universities.ContainsKey(key))
            {
                _universities[key] = new University(name, country);
            }

            return _universities[key];
        }
    }
}
