using Dream_Bridge.Models.Main;

namespace Dream_Bridge.Builders
{
    public class SchoolQueryBuilder
    {
        private IQueryable<School> _query;

        public SchoolQueryBuilder(IQueryable<School> query)
        {
            _query = query;
        }

        public SchoolQueryBuilder FilterByCountry(string country)
        {
            if (!string.IsNullOrEmpty(country))
            {
                _query = _query.Where(s => s.Nation == country);
            }
            return this;
        }

        public SchoolQueryBuilder FilterByCity(string city)
        {
            if (!string.IsNullOrEmpty(city))
            {
                _query = _query.Where(s => s.StateCity == city);
            }
            return this;
        }

        public SchoolQueryBuilder FilterByLevel(string level)
        {
            if (!string.IsNullOrEmpty(level))
            {
                _query = _query.Where(s => s.Level == level);
            }
            return this;
        }

        public SchoolQueryBuilder FilterByMaxTuition(decimal? maxTuition)
        {
            if (maxTuition.HasValue)
            {
                _query = _query.Where(s => s.AverageTuition <= maxTuition);
            }
            return this;
        }


        public IQueryable<School> Build()
        {
            return _query;
        }
    }
}
