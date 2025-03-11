public class USStudyAbroadFactory : IStudyAbroadFactory
{
    public IScholarshipService CreateScholarshipService()
    {
        return new USScholarshipService();
    }

    public IVisaService CreateVisaService()
    {
        return new USVisaService();
    }

    public ISchoolSelectionService CreateSchoolSelectionService()
    {
        return new USSchoolSelectionService();
    }
}
