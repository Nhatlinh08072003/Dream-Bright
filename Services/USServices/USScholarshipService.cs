namespace Dream_Bright.Services
{
    public class USSchoolSelectionService : ISchoolSelectionService
    {
        public string ProvideSchoolSelectionInfo()
        {
            return "Thông tin chọn trường du học Mỹ:\n" +
                   "- Danh sách trường top\n" +
                   "- Yêu cầu đầu vào\n" +
                   "- Chi phí học tập";
        }
    }
}