namespace Dream_Bridge.Models.Main
{
    public class RegistrationRequest
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string ConsultingType { get; set; } = null!;
        public string ContentConsulting { get; set; } = null!;
        public string? Note { get; set; }
    }
}
