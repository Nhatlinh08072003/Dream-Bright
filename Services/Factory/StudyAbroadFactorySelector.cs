using System;

namespace DreamBright.Services.Factory
{
    public class StudyAbroadFactorySelector : IStudyAbroadFactorySelector
    {
        private readonly USStudyAbroadFactory _usFactory;
        private readonly UKStudyAbroadFactory _ukFactory;

        public StudyAbroadFactorySelector(
            USStudyAbroadFactory usFactory,
            UKStudyAbroadFactory ukFactory)
        {
            _usFactory = usFactory ?? throw new ArgumentNullException(nameof(usFactory));
            _ukFactory = ukFactory ?? throw new ArgumentNullException(nameof(ukFactory));
        }

        public IStudyAbroadFactory GetFactory(string country)
        {
            if (string.IsNullOrEmpty(country))
                throw new ArgumentException("Country cannot be null or empty");

            return country.ToUpper() switch
            {
                "US" => _usFactory,
                "UK" => _ukFactory,
                _ => throw new ArgumentException($"Không hỗ trợ quốc gia: {country}")
            };
        }
    }
}
