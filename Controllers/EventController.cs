using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Models.Observer;

namespace Dream_Bridge.Controllers
{
    public class EventController : Controller
    {
        private readonly EventNotifier _eventNotifier;

        public EventController(EventNotifier eventNotifier)
        {
            _eventNotifier = eventNotifier;
        }

        // Hiển thị form nhập thông tin sự kiện
        public IActionResult CreateEvent()
        {
            return View();
        }

        // Xử lý khi người dùng tạo sự kiện
        [HttpPost]
        public IActionResult CreateEvent(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                ViewBag.Message = "⚠️ Tên sự kiện không được để trống.";
                return View();
            }

            _eventNotifier.Notify(message);
            return Ok("Event created and notifications sent");
        }
    }
}
