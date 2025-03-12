using Dream_Bridge.Services.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dream_Bridge.Controllers
{
    public class NotificationController : Controller
    {
        private readonly NotificationService _notificationService;

        public NotificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
        {
            try
            {
                // Gửi thông báo tới một user cụ thể
                await _notificationService.NotifyAsync(
                    request.Message,
                    request.Type,
                    request.UserId
                );
                return Json(new { success = true, message = "Notification sent successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("broadcast")]
        public async Task<IActionResult> Broadcast([FromBody] NotificationRequest request)
        {
            try
            {
                // Gửi thông báo tới tất cả users
                await _notificationService.NotifyAsync(
                    request.Message,
                    request.Type
                );
                return Json(new { success = true, message = "Broadcast sent successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("admin/test-notification")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult TestNotification()
        {
            return View("~/Views/Admin/TestNotification.cshtml");
        }
    }

    public class NotificationRequest
    {
        public string Message { get; set; }
        public string Type { get; set; } // success, info, warning, error
        public string? UserId { get; set; }
    }
}
