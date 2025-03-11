using DreamBright.Services;

namespace DreamBright.Services.Factory
{
    public interface IStudyAbroadFactory
    {
        IScholarshipService CreateScholarshipService();
        IVisaService CreateVisaService();
        ISchoolSelectionService CreateSchoolSelectionService();
    }
}
