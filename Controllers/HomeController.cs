
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Models;
using Dream_Bridge.Models.Main;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Dream_Bridge.Controllers;

public class HomeController : Controller
{
    private readonly StudyAbroadDbContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, StudyAbroadDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult VeChungToi()
    {
        return View();
    }
    public IActionResult DuHoc()
    {
        return View();
    }
    public IActionResult TinTuc()
    {
        var news = _context.News.ToList(); // Lấy danh sách trường từ cơ sở dữ liệu
        return View(news);
    }

    public IActionResult Privacy()
    {
        return View();
    }
   public IActionResult Chat()
    {
       if (User.IsInRole("Admin") || User.IsInRole("Staff"))
{
    return RedirectToAction("qlchat", "Admin"); // Chuyển hướng đến trang quản lý chat
}

        // Lấy ID của người dùng hiện tại
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            return Unauthorized("User not authenticated.");
        }

        ViewData["AdminId"] = 1; // Assuming 1 is the Admin's ID, change it if needed

        // Lấy các tin nhắn mà người dùng hiện tại là người gửi hoặc người nhận
        var chatMessages = _context.ChatMessages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Where(m => (m.SenderId == userId && m.ReceiverId == 1) || (m.SenderId == 1 && m.ReceiverId == userId))
            .OrderBy(m => m.CreatedAt) // Sắp xếp tin nhắn theo thời gian
            .ToList();

        return View(chatMessages);
    }
[HttpPost]
public async Task<IActionResult> DeleteChatMessage(int messageId)
{
    // Retrieve the message
    var message = await _context.ChatMessages.FindAsync(messageId);
    if (message == null)
    {
        return NotFound("Message not found.");
    }

    // Ensure only the sender can delete their own messages
    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
    if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId) || message.SenderId != userId)
    {
        return Unauthorized("You can only delete your own messages.");
    }

    // Remove the message
    _context.ChatMessages.Remove(message);
    await _context.SaveChangesAsync();

    return RedirectToAction("Chat"); // Or you can return a JSON response for AJAX handling
}

    [HttpPost]
    public async Task<IActionResult> SendChatMessage(string messageText, int receiverId, IFormFile? attachment)
    {
        // Kiểm tra nếu người dùng đã đăng nhập
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account"); // Chuyển hướng đến trang đăng nhập
        }

        // Ghi log thông tin nhận được từ yêu cầu
        _logger.LogInformation("Received message: {MessageText}, ReceiverId: {ReceiverId}, Attachment: {Attachment}", messageText, receiverId, attachment?.FileName);

        // Kiểm tra nếu messageText rỗng
        if (string.IsNullOrEmpty(messageText))
        {
            return BadRequest("Message text cannot be empty.");
        }

        // Kiểm tra xem người dùng đã xác thực chưa
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int senderId))
        {
            _logger.LogWarning("User is not authenticated. Claims: {Claims}", User.Claims);
            return Unauthorized("User not authenticated.");
        }

        // Kiểm tra receiverId có hợp lệ không
        var receiver = await _context.Users.FindAsync(receiverId);
        if (receiver == null)
        {
            return NotFound("Receiver not found.");
        }

        string attachmentUrl = null;

        // Nếu có tệp đính kèm, lưu tệp lên thư mục
        if (attachment != null)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

            // Tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Tránh ghi đè tệp nếu có tên trùng nhau
            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(attachment.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await attachment.CopyToAsync(stream);
            }

            attachmentUrl = "/uploads/" + uniqueFileName; // Đường dẫn tới tệp trong thư mục wwwroot
        }

        // Tạo một đối tượng ChatMessage mới
        var chatMessage = new ChatMessage
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            MessageText = messageText,
            CreatedAt = DateTime.Now,
            AttachmentUrl = attachmentUrl // Gán URL tệp đính kèm
        };

        // Thêm tin nhắn vào cơ sở dữ liệu
        _context.ChatMessages.Add(chatMessage);
        await _context.SaveChangesAsync();

        // Chuyển hướng về trang chat
        return RedirectToAction("Chat");
    }
    public IActionResult DichVu()
    {
        return View();
    }
    public IActionResult TimTruong()
    {
        var schools = _context.Schools.ToList(); // Lấy danh sách trường từ cơ sở dữ liệu
        return View(schools);
    }

    [HttpPost("api/schools")]
    public IActionResult FilterSchools([FromBody] FilterViewModel filter)
    {
        var filteredSchools = _context.Schools
            .Where(s => (string.IsNullOrEmpty(filter.Country) || s.Nation == filter.Country) &&
                        (string.IsNullOrEmpty(filter.City) || s.StateCity == filter.City) &&
                        (string.IsNullOrEmpty(filter.EducationLevel) || s.Level == filter.EducationLevel))
            .ToList();

        return Json(filteredSchools); // Trả về JSON
    }

    [HttpGet("api/nations")]
    public IActionResult GetNations()
    {
        var nations = _context.Schools.Select(s => s.Nation).Distinct().ToList();
        return Json(nations);
    }

    [HttpGet("api/cities/{nation}")]
    public IActionResult GetCities(string nation)
    {
        var cities = _context.Schools.Where(s => s.Nation == nation).Select(s => s.StateCity).Distinct().ToList();
        return Json(cities);
    }

    [HttpGet("api/education-levels/{nation}")]
    public IActionResult GetEducationLevels(string nation)
    {
        var educationLevels = _context.Schools.Where(s => s.Nation == nation).Select(s => s.Level).Distinct().ToList();
        return Json(educationLevels);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
