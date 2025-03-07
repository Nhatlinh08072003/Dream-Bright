namespace Dream_Bridge.Model.Main
{
    public interface IChatPermissionService
    {
        bool CanUserAccessChat(int userId);
        void UpdateChatPermission(int userId, bool canAccessChat);
    }
}


