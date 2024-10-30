using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Models;
using Dream_Bridge.Models.Main;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Dream_Bridge.ViewModels;
using System.Security.Claims;
using BCrypt.Net;
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
            if (User.IsInRole("Staff"))
            {
                return RedirectToAction("QLTuvan");
            }

            var users = _studyAbroadDbContext.Users.ToList();
            var viewModel = new CombinedViewModel
            {
                Users = users
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> QLTaikhoan(QLTKViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _studyAbroadDbContext.Users.AnyAsync(u => u.Email == model.Email))
                {
                    TempData["ErrorMessage"] = "Email đã được sử dụng.";
                    var users = await _studyAbroadDbContext.Users.ToListAsync();
                    var viewModel = new CombinedViewModel
                    {
                        Users = users,
                        QLTKViewModel = model
                    };

                    return View(viewModel);
                }

                var user = new User
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    Password = model.Password, // Mã hóa mật khẩu
                    Role = model.Role,
                    IsConsultant = false,
                    ConsultingStatus = "Chưa tư vấn",
                    CreatedAt = DateTime.Now
                };

                _studyAbroadDbContext.Users.Add(user);
                _studyAbroadDbContext.SaveChanges();

                TempData["SuccessMessage"] = "Tạo tài khoản thành công!";
                return RedirectToAction("QLTaikhoan");
            }

            return View(model);
        }
        public IActionResult QLTuvan()
        {
            var consultingRegistrations = _studyAbroadDbContext.ConsultingRegistrations.ToList();
            return View(consultingRegistrations);
            return View();
        }
        [HttpPost]
        public IActionResult UpdateStatus(int id, string status)
        {
            var registration = _studyAbroadDbContext.ConsultingRegistrations.Find(id);
            if (registration != null)
            {
                registration.Status = status;
                _studyAbroadDbContext.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }

        public IActionResult QLTintuc()
        {
            if (User.IsInRole("Staff"))
            {
                return RedirectToAction("QLTuvan");
            }

            return View();
        }

        public IActionResult QLTruong()
        {
            if (User.IsInRole("Staff"))
            {
                return RedirectToAction("QLTuvan");
            }

            return View();
        }

        public IActionResult Index()
        {
            var users = _studyAbroadDbContext.Users.ToList();
            var emailHistories = _studyAbroadDbContext.EmailHistories.ToList();

            var viewModel = new IndexViewModel
            {
                Users = users,
                EmailHistories = emailHistories,
                Message = "Dữ liệu người dùng đã được tải thành công!"
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult QLChat()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Login", "Account"); // Chuyển hướng đến trang đăng nhập
            }

            var chatMessages = _studyAbroadDbContext.ChatMessages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .ToList();

            var users = _studyAbroadDbContext.Users
                .Select(u => new { u.IdUser, u.FullName })
                .ToList();

            ViewData["Users"] = users;

            return View(chatMessages);
        }


        [HttpPost("api/chat/send")]
        public async Task<IActionResult> SendChatMessage(string messageText, int receiverId)
        {
            if (!string.IsNullOrEmpty(messageText))

            {
                var adminIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                int senderId = adminIdClaim != null && int.TryParse(adminIdClaim.Value, out int id) ? id : 0;

                var chatMessage = new ChatMessage
                {
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    MessageText = messageText,
                    CreatedAt = DateTime.Now
                };

                _studyAbroadDbContext.ChatMessages.Add(chatMessage);
                await _studyAbroadDbContext.SaveChangesAsync();

                return Json(new { success = true, message = chatMessage });

            }

            return Json(new { success = false });
        }

        public IActionResult GetChatMessages(int userId)
        {
            var messages = _studyAbroadDbContext.ChatMessages
                .Include(cm => cm.Sender)
                .Include(cm => cm.Receiver)
                .Where(cm => cm.SenderId == userId || cm.ReceiverId == userId)
                .ToList();

            return Json(messages);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult QLDanhMuc()
        {
            if (User.IsInRole("Staff"))
            {
                return RedirectToAction("QLTuvan");
            }

            var categories = _studyAbroadDbContext.StudyAbroadCatalogs.ToList();
            var viewModel = new StudyAbroadCategoryViewModel
            {
                StudyAbroadCatalogs = categories
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> QLDanhMuc(StudyAbroadCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    var category = new StudyAbroadCatalog
                    {
                        NamecategoryStab = model.NamecategoryStab,
                        IdUser = userId,
                    };

                    _studyAbroadDbContext.StudyAbroadCatalogs.Add(category);
                    _studyAbroadDbContext.SaveChanges();

                    TempData["SuccessMessage"] = "Tạo danh mục du học thành công!";
                    return RedirectToAction("QLDanhMuc");
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
