using Dream_Bright.Services.Interfaces;

namespace Dream_Bright.Services
{
    public class USVisaServiceNew : IVisaService
    {
        public string ProvideVisaInfo()
        {
            return "Thông tin visa du học Mỹ:\n" +
                   "- Visa F-1 cho sinh viên\n" +
                   "- Quy trình xin visa\n" +
                   "- Yêu cầu hồ sơ";
        }
    }
}