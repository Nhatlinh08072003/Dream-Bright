
using Dream_Bridge.Models.Main;
using System.Linq;

namespace Dream_Bridge.Builders
{
    public class SchoolQueryBuilder
    {
        private IQueryable<School> _query;

        /*************  ✨ Codeium Command ⭐  *************/
        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolQueryBuilder"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        /// 
        /// <remarks>
        /// The query is expected to be a LINQ query that returns a sequence of <see cref="School"/> objects.
        /// </remarks>
        /******  25d3fec2-5e24-4e69-aad8-15df1e09524d  *******/
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
                _query = _query.Where(s => s.AverageTuition <= maxTuition.Value);
            }
            return this;
        }

        public IQueryable<School> Build()
        {
            return _query;
        }
    }
}
