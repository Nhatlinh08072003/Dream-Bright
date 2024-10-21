using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Models;
using Dream_Bridge.Models.Main;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Data.SqlClient;

namespace Dream_Bridge.Controllers
{
    [Authorize(Roles = "Admin, Staff")] // Cho phép cả Admin và Staff truy cập
    public class AdminController : Controller
    {
        private readonly StudyAbroadDbContext _studyAbroadDbContext;
        private readonly ILogger<AdminController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public AdminController(ILogger<AdminController> logger, StudyAbroadDbContext studyAbroadDbContext, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _studyAbroadDbContext = studyAbroadDbContext;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
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

        [HttpGet]
        public IActionResult QLTruong()
        {
            // Nếu là Staff, chuyển hướng về QLTuvan
            if (User.IsInRole("Staff"))
            {
                return RedirectToAction("QLTuvan");
            }
            // ViewBag.Categories = _studyAbroadDbContext.StudyAbroadCatalogs.ToList(); // Giả sử bạn có bảng StudyAbroadCatalogs
            var categories = _studyAbroadDbContext.StudyAbroadCatalogs.ToList();
            ViewBag.Categories = categories;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCatalog([FromForm] string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return Json(new { success = false, message = "Tên danh mục không được để trống" });
                }

                // Tạo danh mục mới
                var newCatalog = new StudyAbroadCatalog
                {
                    NamecategoryStab = name,
                    IdUser = GetCurrentUserId() // Lấy ID của người dùng hiện tại
                };

                // Thêm vào cơ sở dữ liệu
                _studyAbroadDbContext.StudyAbroadCatalogs.Add(newCatalog);
                await _studyAbroadDbContext.SaveChangesAsync();

                return Json(new { success = true, message = "Danh mục đã được thêm thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        private int GetCurrentUserId()
        {
            throw new NotImplementedException();
        }

        [HttpPost("/admin/AddSchool")]
        public async Task<IActionResult> AddSchool(
[FromForm] int school_id,
[FromForm] string school_title,
[FromForm] string school_nation,
[FromForm] string school_statecity,
[FromForm] string school_description,
[FromForm] string school_level,
[FromForm] decimal school_tuition,
[FromForm] IFormFile image,
[FromForm] int studyAbroadCatalog)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string imageUrl = await SaveImageAsync(image); // Lưu ảnh và nhận URL

                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("localDB")))
                    {
                        await connection.OpenAsync();

                        string sql = @"
                INSERT INTO School (IdcategoryStab, TitleSchool, Nation, StateCity, SchoolDescription, ImageSchool, Level, AverageTuition)
                VALUES (@IdcategoryStab, @TitleSchool, @Nation, @StateCity, @SchoolDescription, @ImageSchool, @Level, @AverageTuition)";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@IdcategoryStab", studyAbroadCatalog);
                            command.Parameters.AddWithValue("@TitleSchool", school_title);
                            command.Parameters.AddWithValue("@Nation", school_nation);
                            command.Parameters.AddWithValue("@StateCity", school_statecity);
                            command.Parameters.AddWithValue("@SchoolDescription", school_description);
                            command.Parameters.AddWithValue("@ImageSchool", imageUrl); // Sử dụng URL thay vì file
                            command.Parameters.AddWithValue("@Level", school_level);
                            command.Parameters.AddWithValue("@AverageTuition", school_tuition);
                            await command.ExecuteNonQueryAsync();
                        }
                    }

                    return Json(new { success = true, message = "Trường học đã được thêm thành công." });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return Json(new { success = false, message = $"Lỗi khi lưu trường học: {ex.Message}" });
                }
            }
            else
            {
                // Ghi lại các lỗi nếu có
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
        }

        public async Task<string> SaveImageAsync(IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                // Kiểm tra định dạng tệp
                var fileExtension = Path.GetExtension(image.FileName).ToLower();
                if (fileExtension != ".png")
                {
                    throw new InvalidOperationException("Chỉ hỗ trợ tệp hình ảnh PNG.");
                }

                // Đường dẫn lưu hình ảnh
                var fileName = Path.GetFileName(image.FileName);
                var filePath = Path.Combine("wwwroot/images", fileName); // Thay đổi đường dẫn theo yêu cầu

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                return $"/images/{fileName}"; // Trả về URL để lưu vào cơ sở dữ liệu
            }

            throw new InvalidOperationException("Không có hình ảnh nào được tải lên.");
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
