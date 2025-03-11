namespace DreamBright.Services.Factory
{
    public interface IStudyAbroadFactorySelector
    {
        IStudyAbroadFactory GetFactory(string country);
    }
}
