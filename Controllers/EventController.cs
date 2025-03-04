using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Models.Observer;

namespace Dream_Bridge.Controllers
{
    public class EventController : Controller
    {
        private readonly EventNotifier _notifier;

        public EventController(EventNotifier notifier)
        {
            _notifier = notifier;
        }

        // Hiển thị form nhập thông tin sự kiện
        public IActionResult CreateEvent()
        {
            return View();
        }

        // Xử lý khi người dùng tạo sự kiện
        [HttpPost]
        public IActionResult CreateEvent(string eventName)
        {
            if (string.IsNullOrWhiteSpace(eventName))
            {
                ViewBag.Message = "⚠️ Tên sự kiện không được để trống.";
                return View();
            }

            string message = $"🔥 Sự kiện mới: {eventName}";
            _notifier.Notify(message);

            ViewBag.Message = $"✅ Đã gửi thông báo: {eventName}";
            return View();
        }
    }
}
