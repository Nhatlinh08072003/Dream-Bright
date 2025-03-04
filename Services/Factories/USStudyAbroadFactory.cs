public class USStudyAbroadFactory : IStudyAbroadFactory
{
    public IScholarshipService CreateScholarshipService() => new USScholarshipService();
    public IVisaService CreateVisaService() => new USVisaService();
    public ISchoolSelectionService CreateSchoolSelectionService() => new USSchoolSelectionService();
}
