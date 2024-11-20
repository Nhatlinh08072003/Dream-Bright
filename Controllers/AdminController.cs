using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Models;
using Dream_Bridge.Models.Main;
using Microsoft.AspNetCore.Authorization;
// using Microsoft.EntityFrameworkCore;
using Dream_Bridge.ViewModels;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Dream_Bridge.Hubs;
// using BCrypt.Net;
namespace Dream_Bridge.Controllers
{
    [Authorize(Roles = "Admin, Staff")] // Cho phép cả Admin và Staff truy cập  
    public class AdminController : Controller
    {
            private readonly IHubContext<ChatHub> _hubContext; // Inject IHubContext

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly StudyAbroadDbContext _studyAbroadDbContext;
        private readonly ILogger<AdminController> _logger;

        public AdminController( IHubContext<ChatHub> hubContext,ILogger<AdminController> logger, StudyAbroadDbContext studyAbroadDbContext, IWebHostEnvironment webHostEnvironment)
        {
              _hubContext = hubContext;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _studyAbroadDbContext = studyAbroadDbContext;
        }
        private int GetCurrentUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return 0;
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
            [HttpPost("api/admin/send-notification")]
public async Task<IActionResult> SendNotification([FromBody] Notification notificationDto)
{
    try
    {
        var users = await _studyAbroadDbContext.Users.ToListAsync();  // Lấy tất cả người dùng từ cơ sở dữ liệu
        var notifications = users.Select(user => new Notification
        {
            UserId = user.IdUser,
            Title = notificationDto.Title,
            Message = notificationDto.Message,
            CreatedAt = DateTime.Now,
            IsRead = false
        }).ToList();

        // Thêm tất cả thông báo vào cơ sở dữ liệu
        await _studyAbroadDbContext.Notifications.AddRangeAsync(notifications);
        await _studyAbroadDbContext.SaveChangesAsync();

        // Lưu lịch sử thông báo đã gửi
        var emailHistories = users.Select(user => new EmailHistory
        {
            ToEmail = user.Email,
            Subject = notificationDto.Title,
            Body = notificationDto.Message,
            SentAt = DateTime.Now
        }).ToList();

        // Thêm lịch sử gửi email vào cơ sở dữ liệu
        await _studyAbroadDbContext.EmailHistories.AddRangeAsync(emailHistories);
        await _studyAbroadDbContext.SaveChangesAsync();

        // Gửi thông báo qua SignalR cho tất cả người dùng
        foreach (var notification in notifications)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
        }

        return Ok(new { message = "Notification sent to all users." });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = "An error occurred while sending notifications.", details = ex.Message });
    }
}




        public IActionResult QLTuvan()
        {
            var consultingRegistrations = _studyAbroadDbContext.ConsultingRegistrations.ToList();
            return View(consultingRegistrations);
            // return View();
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
        [HttpGet]
        public IActionResult QLTinTuc()
        {
            if (User.IsInRole("Staff"))
            {
                return RedirectToAction("QLTuvan");
            }

            // Fetch School data from database context (_studyAbroadDbContext)
            var newsViewModel = new NewsViewModel
            {
                NewsList = _studyAbroadDbContext.News.ToList() // Use News here
            };

            return View(newsViewModel);
        }




        [HttpPost]
        public async Task<IActionResult> QLTinTuc(NewsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                int userId = userIdClaim != null && int.TryParse(userIdClaim.Value, out int id) ? id : 0;

                string? imagePath = null;

                // Process the uploaded file
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    Directory.CreateDirectory(uploadsFolder); // Ensure directory exists

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream); // Save the uploaded file
                    }
                    imagePath = "/images/" + uniqueFileName; // Set the image path for the database
                }

                // Create a new news object
                var news = new News
                {
                    TitleNews = model.TitleNews,
                    NewsDescription = model.NewsDescription,
                    NewsContent = model.NewsContent,
                    NewsImage = imagePath, // Assign the image path here
                    IdUser = userId,
                    CreatedAt = DateTime.Now // You can also use DateTime.UtcNow for UTC time
                };

                // Save the new news to the database
                _studyAbroadDbContext.News.Add(news);
                await _studyAbroadDbContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Tin tức đã được thêm thành công!";
                return RedirectToAction("QLTintuc"); // Redirect to the same action or another action
            }

            // Return to the view with model errors if validation fails
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateNews(NewsViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Fetch the existing school from the database using the ID
                var news = _studyAbroadDbContext.News.Find(model.IdNews);
                if (news != null)
                {
                    // Update the school's properties with the form data
                    news.TitleNews = model.TitleNews;
                    news.NewsDescription = model.NewsDescription;
                    news.NewsContent = model.NewsContent;

                    if (model.ImageFile != null)
                    {
                        try
                        {
                            // Validate the image file (optional: add file size/type checks)
                            var fileExtension = Path.GetExtension(model.ImageFile.FileName);
                            if (fileExtension == ".jpg" || fileExtension == ".png" || fileExtension == ".jpeg")
                            {
                                // Handle the image upload
                                var fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName) + "_" + Guid.NewGuid() + fileExtension;
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);

                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    model.ImageFile.CopyTo(stream);
                                }

                                // Update the ImageSchool property
                                news.NewsImage = "/Images/" + fileName;
                            }
                            else
                            {
                                TempData["ErrorMessage"] = "Invalid image format. Only .jpg, .jpeg, and .png are allowed.";
                                return View(model);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log the error (not shown here)
                            TempData["ErrorMessage"] = "Image upload failed: " + ex.Message;
                            return View(model);
                        }
                    }

                    _studyAbroadDbContext.SaveChanges(); // Save the changes to the database
                    TempData["SuccessMessage"] = "Cập nhật thông tin tin tức thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy thông tin tin tức!";
                }

                return RedirectToAction("QLTinTuc"); // Redirect to the list view after updating
            }

            // If model state is invalid, return the same view with the model data
            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteNews(int id)
        {
            var news = _studyAbroadDbContext.News.Find(id); // Use News here
            if (news != null)
            {
                _studyAbroadDbContext.News.Remove(news);
                _studyAbroadDbContext.SaveChanges();
                TempData["SuccessMessage"] = "Xóa tin tức thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin tin tức!";
            }

            return RedirectToAction("QLTintuc");
        }

        public IActionResult QLChat()
        {
            // if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin")||!User.IsInRole("Staff") )
            // {
            //     return RedirectToAction("qlchat", "admin"); 
            //     return RedirectToAction("qlchat", "staff"); 
            // }


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
        public async Task<IActionResult> SendChatMessage(string messageText, int receiverId, IFormFile? attachment)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("User not authenticated.");
            }

            var senderIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            int senderId = senderIdClaim != null && int.TryParse(senderIdClaim.Value, out int id) ? id : 0;

            if (string.IsNullOrEmpty(messageText) && attachment == null)
            {
                return BadRequest("Message text or attachment must be provided.");
            }

            string attachmentUrl = null;

            // Handle file upload if an attachment is present
            if (attachment != null)
            {
                var fileName = Path.GetFileName(attachment.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await attachment.CopyToAsync(stream);
                }
                attachmentUrl = "/uploads/" + fileName;
            }

            var chatMessage = new ChatMessage
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                MessageText = messageText,
                CreatedAt = DateTime.Now,
                AttachmentUrl = attachmentUrl // Save the URL of the uploaded file
            };

            _studyAbroadDbContext.ChatMessages.Add(chatMessage);
            await _studyAbroadDbContext.SaveChangesAsync();

            return Json(new { success = true, message = chatMessage });
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
                    DetailedDescription = model.DetailedDescription,
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
        [HttpPost]
        public IActionResult UpdateSchool(SchoolViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Fetch the existing school from the database using the ID
                var school = _studyAbroadDbContext.Schools.Find(model.IdSchool);
                if (school != null)
                {
                    // Update the school's properties with the form data
                    school.TitleSchool = model.TitleSchool;
                    school.SchoolDescription = model.SchoolDescription;
                    school.Nation = model.Nation;
                    school.StateCity = model.StateCity;
                    school.AverageTuition = model.AverageTuition;
                    school.Level = model.Level;
                    school.DetailedDescription = model.DetailedDescription;
                    if (model.ImageFile != null)
                    {
                        try
                        {
                            // Validate the image file (optional: add file size/type checks)
                            var fileExtension = Path.GetExtension(model.ImageFile.FileName);
                            if (fileExtension == ".jpg" || fileExtension == ".png" || fileExtension == ".jpeg")
                            {
                                // Handle the image upload
                                var fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName) + "_" + Guid.NewGuid() + fileExtension;
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);

                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    model.ImageFile.CopyTo(stream);
                                }

                                // Update the ImageSchool property
                                school.ImageSchool = "/Images/" + fileName;
                            }
                            else
                            {
                                TempData["ErrorMessage"] = "Invalid image format. Only .jpg, .jpeg, and .png are allowed.";
                                return View(model);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log the error (not shown here)
                            TempData["ErrorMessage"] = "Image upload failed: " + ex.Message;
                            return View(model);
                        }
                    }

                    _studyAbroadDbContext.SaveChanges(); // Save the changes to the database
                    TempData["SuccessMessage"] = "Cập nhật thông tin trường thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy thông tin trường!";
                }

                return RedirectToAction("QLTruong"); // Redirect to the list view after updating
            }

            // If model state is invalid, return the same view with the model data
            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteSchool(int id)
        {
            var school = _studyAbroadDbContext.Schools.Find(id);
            if (school != null)
            {
                _studyAbroadDbContext.Schools.Remove(school);
                _studyAbroadDbContext.SaveChanges();
                TempData["SuccessMessage"] = "Xóa trường thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin trường!";
            }

            return RedirectToAction("QLTruong");
        }

        public IActionResult Index()
        {
            var users = _studyAbroadDbContext.Users.ToList();
            var consultingregistration = _studyAbroadDbContext.ConsultingRegistrations.ToList();
            var school = _studyAbroadDbContext.Schools.ToList();

            var emailHistories = _studyAbroadDbContext.EmailHistories.ToList();

            var viewModel = new IndexViewModel
            {
                Users = users,
                ConsultingRegistrations = consultingregistration,
                Schools = school,
                EmailHistories = emailHistories,
                Message = "Dữ liệu người dùng đã được tải thành công!"
            };

            return View(viewModel);
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

    [Serializable]
    internal class DbUpdateException : Exception
    {
        public DbUpdateException()
        {
        }

        public DbUpdateException(string? message) : base(message)
        {
        }

        public DbUpdateException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}