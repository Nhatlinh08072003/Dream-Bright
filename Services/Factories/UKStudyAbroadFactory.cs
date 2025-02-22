public class UKStudyAbroadFactory : IStudyAbroadFactory
{
    public IScholarshipService CreateScholarshipService() => new UKScholarshipService();
    public IVisaService CreateVisaService() => new UKVisaService();
    public ISchoolSelectionService CreateSchoolSelectionService() => new UKSchoolSelectionService();
}
