using System;

namespace Dream_Bridge.Models.Main
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public int? UserId { get; set; } // null for broadcast
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }

        // Navigation property
        public virtual User User { get; set; }
    }
}
