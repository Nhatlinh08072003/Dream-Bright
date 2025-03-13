using Dream_Bridge.Model.Main;
using Dream_Bridge.Models.Main;

namespace Dream_Bridge.Services
{
    public class ChatPermissionService : IChatPermissionService
    {
        private readonly StudyAbroadDbContext _context;

        public ChatPermissionService(StudyAbroadDbContext context)
        {
            _context = context;
        }

        public bool CanUserAccessChat(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null) return false;
            
            // Admin always has access
            if (user.Role == "Admin") return true;
            
            // For Staff, check CanAccessChat flag
            if (user.Role == "Staff") return user.CanAccessChat;
            
        // For regular users
            return true;
        }

        public void UpdateChatPermission(int userId, bool canAccessChat)
        {
            var user = _context.Users.Find(userId);
            if (user == null) return;

            user.CanAccessChat = canAccessChat;
            _context.SaveChanges();
        }
    }
}
