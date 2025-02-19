public interface IStudyAbroadFactory
{
    IScholarshipService CreateScholarshipService();
    IVisaService CreateVisaService();
    ISchoolSelectionService CreateSchoolSelectionService();
}
