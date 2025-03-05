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
            throw new NotImplementedException();
        }

        // public bool CanUserAccessChat(int userId)
        // {
        //     var permission = _studyAbroadDbContext.ChatPermissions
        //         .FirstOrDefault(p => p.UserId == userId);

        //     // Người dùng chỉ có thể truy cập nếu cả CanAccessChat và IsActive đều là true (1)
        //     return permission != null && permission.CanAccessChat && permission.IsActive;
        // }



        [HttpPost]
        public IActionResult UpdateChatPermission([FromBody] ChatPermissionModel model)
        {
            if (model == null || model.UserId == 0)
            {
                return new BadRequestObjectResult("Dữ liệu không hợp lệ!");

            }

            var user = _studyAbroadDbContext.Users.Find(model.UserId);
            if (user == null)
            {
                return new NotFoundObjectResult("Người dùng không tồn tại!");

            }

            user.IsActive = model.IsActive; // Cập nhật trạng thái cho phép truy cập QLChat
            _studyAbroadDbContext.SaveChanges();

            return new OkObjectResult("Cập nhật thành công");
        }

        public void UpdateChatPermission(int userId, bool canAccessChat, bool isActive)
        {
            throw new NotImplementedException();
        }
    }
}
