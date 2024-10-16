
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Models;
using Dream_Bridge.Models.Main;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Dream_Bridge.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly StudyAbroadDbContext _context;

    public AccountController(ILogger<AccountController> logger, StudyAbroadDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Login()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Kiểm tra nếu email đã tồn tại
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng.");
                return View(model);
            }

            // Kiểm tra mật khẩu và xác nhận mật khẩu
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Mật khẩu và xác nhận mật khẩu không giống nhau.");
                return View(model);
            }

            // Tạo mới người dùng
            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password), // Mã hóa mật khẩu
                Role = "User", // Hoặc bất kỳ vai trò nào bạn muốn
                IsConsultant = false,
                ConsultingStatus = "Chưa tư vấn", // Tùy chọn, có thể thay đổi
                CreatedAt = DateTime.Now
            };

            // Thêm người dùng vào cơ sở dữ liệu
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đăng ký thành công! Bạn có thể đăng nhập.";
            return RedirectToAction("Login"); // Điều hướng đến trang đăng nhập
        }

        return View(model);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
