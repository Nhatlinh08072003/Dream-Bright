using Microsoft.Extensions.DependencyInjection;

public class StudyAbroadFactorySelector : IStudyAbroadFactorySelector
{
    private readonly IServiceProvider _serviceProvider;

    public StudyAbroadFactorySelector(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IStudyAbroadFactory GetFactory(string country)
    {
        try
        {
            return country switch
            {
                "US" => _serviceProvider.GetRequiredService<USStudyAbroadFactory>(),
                "UK" => _serviceProvider.GetRequiredService<UKStudyAbroadFactory>(),
                _ => throw new Exception("Không hỗ trợ quốc gia này")
            };
        }
        catch (Exception)
        {
            throw new Exception("Không thể tạo factory cho quốc gia này");
        }
    }
}
