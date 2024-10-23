using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Models;
using Dream_Bridge.Models.Main;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Dream_Bridge.ViewModels;
using System.Security.Claims;


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
            var users = _studyAbroadDbContext.Users.ToList(); // Lấy danh sách người dùng
            var emailHistories = _studyAbroadDbContext.EmailHistories.ToList(); // Lấy danh sách email đã gửi

            var viewModel = new IndexViewModel
            {
                Users = users,
                EmailHistories = emailHistories, // Truyền danh sách EmailHistory vào view model
                Message = "Dữ liệu người dùng đã được tải thành công!" // Thông điệp tùy chọn
            };

            return View(viewModel); // Truyền mô hình đến view
        }

        public IActionResult QLTuvan()
        {
            return View();
        }
        public IActionResult QLChat()
        {
            return View();
        }
        public IActionResult QLDanhMuc()
        {
            // Nếu là Staff, chuyển hướng về QLTuvan
            if (User.IsInRole("Staff"))
            {
                return RedirectToAction("QLTuvan");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> QLDanhMuc(StudyAbroadCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Lấy IdUser từ claim
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    // Tạo mới danh mục du học
                    var category = new StudyAbroadCatalog
                    {
                        NamecategoryStab = model.NamecategoryStab,
                        IdUser = userId, // Gán IdUser từ claim
                    };

                    // Thêm danh mục vào cơ sở dữ liệu
                    _studyAbroadDbContext.StudyAbroadCatalogs.Add(category);
                    await _studyAbroadDbContext.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Tạo danh mục du học thành công!";
                    return RedirectToAction("QLDanhMuc"); // Hoặc đến trang danh sách danh mục
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể lấy thông tin người dùng.";
                }
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
