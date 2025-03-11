public class UKStudyAbroadFactory : IStudyAbroadFactory
{
    public IScholarshipService CreateScholarshipService()
    {
        return new UKScholarshipService();
    }

    public IVisaService CreateVisaService()
    {
        return new UKVisaService();
    }

    public ISchoolSelectionService CreateSchoolSelectionService()
    {
        return new UKSchoolSelectionService();
    }
}
