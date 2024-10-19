namespace Dream_Bridge.Models.Main
{
    public class CombinedViewModel
    {
        public List<User> Users { get; set; }
        public QLTKViewModel QLTKViewModel { get; set; } // Thêm thuộc tính QLTKViewModel

        public RegisterViewModel Register { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
