
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Models;
using Dream_Bridge.Models.Main;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Dream_Bridge.ViewModels;
using Dream_Bright.Models;

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
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

            if (user != null && user.Email != null && user.Email != null)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),

            };
                // Chỉ thêm Claim cho Role nếu Role không phải là null
                if (!string.IsNullOrEmpty(user.Role))
                {
                    claims.Add(new Claim(ClaimTypes.Role, user.Role));
                }
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                ViewBag.Email = user.Email;

                if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "Admin"); // Chuyển hướng đến trang admin
                }
                else
                {
                    return RedirectToAction("Index", "Home"); // Chuyển hướng đến trang chủ
                }
            }

            ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác. Vui lòng thử lại.");
        }

        return View(model);
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
                Password = model.Password, // Mã hóa mật khẩu
                Role = "User", // Hoặc bất kỳ vai trò nào bạn muốn
                IsConsultant = false,
                ConsultingStatus = "Chưa tư vấn", // Tùy chọn, có thể thay đổi
                CreatedAt = DateTime.Now
            };

            // Thêm người dùng vào cơ sở dữ liệu
            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Đăng ký thành công! Bạn có thể đăng nhập.";
            return RedirectToAction("Login"); // Điều hướng đến trang đăng nhập
        }

        return View(model);
    }
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account"); // Chuyển hướng về trang Login
    }
    public IActionResult AccessDenied()
    {
        return View();
    }
    // public IActionResult Profile()
    // {
    //     return View();
    // }
    public IActionResult Profile()
    {
        //tim nguoi dung
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // kiểm tra người dùng đã đăng nhập chưa
        if (userId == null)
        {
            return RedirectToAction("Login");
        }

        var user = _context.Users.Find(int.Parse(userId));
        // tìm người dùng trong cơ sở dữ liệu
        if (user == null)
        {
            return NotFound();
        }

        // Trả về view với model là đối tượng user
        return View(user);
    }



    public IActionResult PageAcc()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return RedirectToAction("Login");
        }

        var user = _context.Users.Find(int.Parse(userId));

        if (user == null)
        {
            return NotFound();
        }

        ViewBag.FullName = user.FullName; // Truyền tên người dùng vào ViewBag để hiển thị trong view
        return View();
    }

    public IActionResult ResetPassword()
    {
        var model = new ResetPasswordModel(); // Tạo một instance của ResetPasswordModel
        return View(model); // Truyền đúng model vào view
    }

    // Phương thức xử lý yêu cầu đổi mật khẩu
    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
    {
        if (ModelState.IsValid)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(int.Parse(userId));

            if (user == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            // Kiểm tra mật khẩu hiện tại
            if (user.Password != model.CurrentPassword)
            {
                ModelState.AddModelError("CurrentPassword", "Mật khẩu hiện tại không chính xác.");
                return View(model); // Trả về view để cho phép nhập lại mật khẩu
            }

            // Kiểm tra mật khẩu mới và xác nhận
            if (model.NewPassword != model.ConfirmNewPassword)
            {
                ModelState.AddModelError("ConfirmNewPassword", "Mật khẩu mới và xác nhận mật khẩu không giống nhau.");
                return View(model);
            }

            // Cập nhật mật khẩu mới
            user.Password = model.NewPassword;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
            return RedirectToAction("Index", "Home");
        }

        return View(model);
    }


    [HttpGet]
    public IActionResult UpdateProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return RedirectToAction("Login");
        }

        var user = _context.Users.Find(int.Parse(userId));

        if (user == null)
        {
            return NotFound();
        }

        // Truyền tên đầy đủ vào ViewBag
        ViewBag.FullName = user.FullName;

        // Tạo ViewModel để đẩy dữ liệu người dùng hiện tại vào form
        var model = new UpdateProfileViewModel
        {
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address
        };

        return View(model);
    }



    // Xử lý việc cập nhật thông tin:
    [HttpPost]
    public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model)
    {
        if (ModelState.IsValid)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var user = await _context.Users.FindAsync(int.Parse(userId));

            if (user == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            // Kiểm tra email đã tồn tại hay chưa (ngoại trừ email của người dùng hiện tại)
            if (await _context.Users.AnyAsync(u => u.Email == model.Email && u.IdUser != user.IdUser))
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng bởi người dùng khác.");
                return View(model);
            }

            // Cập nhật thông tin người dùng
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;

            // Lưu thay đổi
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật thông tin cá nhân thành công!";
            return RedirectToAction("Profile");
        }

        return View(model);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

internal class ResetPassword
{
    public ResetPassword()
    {
    }
}