<<<<<<< HEAD
using System.Diagnostics;  
using Microsoft.AspNetCore.Mvc;  
using Dream_Bridge.Models;  
using Dream_Bridge.Models.Main;  
using Microsoft.AspNetCore.Authorization;  
using Microsoft.EntityFrameworkCore;  
using Dream_Bridge.ViewModels;  
using System.Security.Claims;  
using BCrypt.Net;  
=======
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
>>>>>>> origin/Huu-SCRUM-6

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
                await _studyAbroadDbContext.SaveChangesAsync();  

                TempData["SuccessMessage"] = "Tạo tài khoản thành công!";  
                return RedirectToAction("QLTaikhoan");  
            }  

            return View(model);  
        }  

<<<<<<< HEAD
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
=======
            return View();
        }
        [HttpGet]
        public IActionResult QLTruong()
        {
            // Check user role
            if (User.IsInRole("Staff"))
            {
                return RedirectToAction("QLTuvan");
            }

            // Fetch School data from database context (_studyAbroadDbContext)
            var schoolViewModel = new SchoolViewModel
            {
                Schools = _studyAbroadDbContext.Schools.ToList() // Assuming _studyAbroadDbContext is your DbContext
            };

            // Return the populated ViewModel to the view
            return View(schoolViewModel);
        }


        [HttpPost]
        public IActionResult QLTruong(SchoolViewModel model)
        {
            // Check if the file is selected
            if (model.ImageFile == null || model.ImageFile.Length == 0)
            {
                ModelState.AddModelError("ImageFile", "Please select an image file.");
            }

            // Debugging information
            Console.WriteLine($"File selected: {model.ImageFile?.FileName}");
            Console.WriteLine($"File length: {model.ImageFile?.Length}");

            // Validate IdcategoryStab
            var categoryExists = _studyAbroadDbContext.StudyAbroadCatalogs
                .Any(c => c.IdcategoryStab == model.IdcategoryStab);

            if (!categoryExists)
            {
                ModelState.AddModelError("IdcategoryStab", "The selected category does not exist.");
            }

            if (ModelState.IsValid)
            {
                string? imagePath = null;

                // Process the uploaded file
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    Directory.CreateDirectory(uploadsFolder); // Ensure directory exists

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    try
                    {
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            model.ImageFile.CopyTo(fileStream); // Save the uploaded file
                        }
                        imagePath = "/images/" + uniqueFileName; // Set the image path for the database
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("ImageFile", "Error uploading image: " + ex.Message);
                        Console.WriteLine($"Error uploading image: {ex}");
                    }
                }

                // Create a new school object
                var school = new School
                {
                    ImageSchool = imagePath,
                    TitleSchool = model.TitleSchool,
                    SchoolDescription = model.SchoolDescription,
                    Nation = model.Nation,
                    StateCity = model.StateCity,
                    AverageTuition = model.AverageTuition,
                    Level = model.Level,
                    IdcategoryStab = model.IdcategoryStab,
                };

                // Save the new school to the database
                try
                {
                    _studyAbroadDbContext.Schools.Add(school);
                    _studyAbroadDbContext.SaveChanges();
                    TempData["SuccessMessage"] = "School added successfully.";
                    return RedirectToAction("QLTruong"); // Redirect to an index page or another action
                }
                catch (DbUpdateException dbEx)
                {
                    ModelState.AddModelError("", "Database error: " + dbEx.InnerException?.Message);
                    Console.WriteLine($"Database error: {dbEx}");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error saving school: " + ex.Message);
                    Console.WriteLine($"Error saving school: {ex}");
                }
            }

            // Return to the view with model errors if validation fails
            return View(model);
        }



        public IActionResult Index()
        {
            var users = _studyAbroadDbContext.Users.ToList();
            var emailHistories = _studyAbroadDbContext.EmailHistories.ToList();
>>>>>>> origin/Huu-SCRUM-6

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

<<<<<<< HEAD
        public IActionResult QLTuvan()  
        {  
            return View();  
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
=======
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
>>>>>>> origin/Huu-SCRUM-6

            return View(chatMessages);  
        }  

<<<<<<< HEAD
        [HttpPost("api/chat/send")]  
        public async Task<IActionResult> SendChatMessage(string messageText, int receiverId)  
        {  
            if (!string.IsNullOrEmpty(messageText))  
            {  
                var adminIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);  
                int senderId = adminIdClaim != null && int.TryParse(adminIdClaim.Value, out int id) ? id : 0;  
=======
        [HttpPost("api/chat/send")]
        public async Task<IActionResult> SendChatMessage(string messageText, int receiverId)
        {
            if (!string.IsNullOrEmpty(messageText))

            {
                var adminIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                int senderId = adminIdClaim != null && int.TryParse(adminIdClaim.Value, out int id) ? id : 0;
>>>>>>> origin/Huu-SCRUM-6

                var chatMessage = new ChatMessage  
                {  
                    SenderId = senderId,  
                    ReceiverId = receiverId,  
                    MessageText = messageText,  
                    CreatedAt = DateTime.Now  
                };  

                _studyAbroadDbContext.ChatMessages.Add(chatMessage);  
                await _studyAbroadDbContext.SaveChangesAsync();  

<<<<<<< HEAD
                // Trả về tin nhắn đã gửi  
                return Json(new { success = true, message = chatMessage });  
            }  
=======
                return Json(new { success = true, message = chatMessage });

            }
>>>>>>> origin/Huu-SCRUM-6

            return Json(new { success = false });  
        }  

        [HttpGet("api/chat/messages/{userId}")]  
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
                    await _studyAbroadDbContext.SaveChangesAsync();  

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