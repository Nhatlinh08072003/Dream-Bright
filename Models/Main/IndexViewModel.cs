// D:\Dream-Bright\Models\IndexViewModel.cs
using Dream_Bridge.Models.Main;

namespace Dream_Bridge.Models
{
    public class IndexViewModel
    {
        public List<User>? Users { get; set; }
        public List<ConsultingRegistration>? ConsultingRegistrations { get; set; }
        public List<School>? Schools { get; set; }


        public List<EmailHistory>? EmailHistories { get; set; } // Thêm danh sách EmailHistory
        public string? Message { get; set; }
        public List<NotificationViewModel> Notifications { get; set; }
    }

}
