namespace DreamBright.Models
{
    public class ConsultingViewModel
    {
        public string Country { get; set; } = string.Empty;
        public string ScholarshipInfo { get; set; } = string.Empty;
        public string VisaInfo { get; set; } = string.Empty;
        public string SchoolSelectionInfo { get; set; } = string.Empty;
        public List<string> Services { get; set; } = new List<string>();
    }
}
