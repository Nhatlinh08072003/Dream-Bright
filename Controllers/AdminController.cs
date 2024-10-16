using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Models;
using Microsoft.AspNetCore.Authorization;

namespace Dream_Bridge.Controllers
{
    [Authorize(Roles = "Admin, Staff")] // Cho phép cả Admin và Staff truy cập
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }

        public IActionResult QLTaikhoan()
        {
            // Kiểm tra xem người dùng có vai trò Staff không
            if (User.IsInRole("Staff"))
            {
                // Nếu là Staff, chuyển hướng về QLTuvan
                return RedirectToAction("QLTuvan");
            }
            return View();
        }

        public IActionResult QLTintuc()
        {
            // Nếu là Staff, chuyển hướng về QLTuvan
            if (User.IsInRole("Staff"))
            {
                return RedirectToAction("QLTuvan");
            }
            return View();
        }

        public IActionResult QLTruong()
        {
            // Nếu là Staff, chuyển hướng về QLTuvan
            if (User.IsInRole("Staff"))
            {
                return RedirectToAction("QLTuvan");
            }
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult QLTuvan()
        {
            return View();
        }

        public IActionResult QLDuHoc()
        {
            // Nếu là Staff, chuyển hướng về QLTuvan
            if (User.IsInRole("Staff"))
            {
                return RedirectToAction("QLTuvan");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
