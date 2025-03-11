namespace DreamBright.Services.UK
{
    public class UKVisaService : IVisaService
    {
        public string ProvideVisaInfo()
        {
            return "Thông tin visa du học UK:\n" +
                   "- Student visa\n" +
                   "- Quy trình xin visa UK\n" +
                   "- Yêu cầu hồ sơ UK";
        }
    }
}
