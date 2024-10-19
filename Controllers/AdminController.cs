using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Models;
using Dream_Bridge.Models.Main;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Dream_Bridge.Controllers
{
    [Authorize(Roles = "Admin, Staff")] // Cho phép cả Admin và Staff truy cập
    public class AdminController : Controller
    {
        private readonly StudyAbroadDbContext _studyAbroadDbContext;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger, StudyAbroadDbContext studyAbroadDbContext)
        {
            _logger = logger;
            _studyAbroadDbContext = studyAbroadDbContext;
        }

        [HttpGet]
        public IActionResult QLTaikhoan()
        {
            // Kiểm tra xem người dùng có vai trò Staff không
            if (User.IsInRole("Staff"))
            {
                // Nếu là Staff, chuyển hướng về QLTuvan
                return RedirectToAction("QLTuvan");
            }
            var users = _studyAbroadDbContext.Users.ToList(); // Lấy danh sách người dùng

            var viewModel = new CombinedViewModel
            {
                Users = users // Giả sử bạn có thuộc tính Users trong CombinedViewModel
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> QLTaikhoan(QLTKViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra nếu email đã tồn tại
                if (await _studyAbroadDbContext.Users.AnyAsync(u => u.Email == model.Email))
                {
                    TempData["ErrorMessage"] = "Email đã được sử dụng.";
                    var users = _studyAbroadDbContext.Users.ToList();
                    var viewModel = new CombinedViewModel
                    {
                        Users = users,
                        QLTKViewModel = model // Đưa model hiện tại vào CombinedViewModel
                    };

                    return View(viewModel);
                }

                // Tạo mới người dùng
                var user = new User
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    Password = model.Password, // Mã hóa mật khẩu
                    Role = model.Role, // Hoặc bất kỳ vai trò nào bạn muốn
                    IsConsultant = false,
                    ConsultingStatus = "Chưa tư vấn", // Tùy chọn, có thể thay đổi
                    CreatedAt = DateTime.Now
                };

                // Thêm người dùng vào cơ sở dữ liệu
                _studyAbroadDbContext.Users.Add(user);
                await _studyAbroadDbContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Tạo tài khoản thành công!";
                return RedirectToAction("QLTaiKhoan"); // Điều hướng đến trang đăng nhập
            }

            return View(model);
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
        public IActionResult QLChat()
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
