using Dream_Bridge.Models.Main;
using Microsoft.AspNetCore.Mvc;

namespace Dream_Bridge.Model.Main
{
    public class ChatPermissionService : IChatPermissionService
    {
        private readonly StudyAbroadDbContext _studyAbroadDbContext;

        public ChatPermissionService(StudyAbroadDbContext context)
        {
            _studyAbroadDbContext = context;
        }

        public bool CanUserAccessChat(int userId)
        {
            var user = _studyAbroadDbContext.Users.FirstOrDefault(u => u.IdUser == userId);
            return user != null && user.CanAccessChat;
        }

        public void UpdateChatPermission(int userId, bool canAccessChat)
        {
            var user = _studyAbroadDbContext.Users.Find(userId);
            if (user != null)
            {
                user.CanAccessChat = canAccessChat;
                _studyAbroadDbContext.SaveChanges();
            }
        }
    }
}


