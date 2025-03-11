namespace DreamBright.Services.UK
{
    public class UKSchoolSelectionService : ISchoolSelectionService
    {
        public string ProvideSchoolSelectionInfo()
        {
            return "Thông tin chọn trường du học UK:\n" +
                   "- Danh sách trường UK\n" +
                   "- Yêu cầu đầu vào UK\n" +
                   "- Chi phí học tập UK";
        }
    }
}
