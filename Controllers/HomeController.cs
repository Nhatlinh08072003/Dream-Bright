
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


    // // Thêm phương thức Chat
    // public IActionResult Chat()
    // {

    //     // Kiểm tra nếu người dùng đã đăng nhập
    //     if (!User.Identity.IsAuthenticated)
    //     {
    //         return RedirectToAction("Login", "Account"); // Chuyển hướng đến trang đăng nhập
    //     }

    //     ViewData["AdminId"] = 1;
    //     var chatMessages = _context.ChatMessages
    //         .Include(m => m.Sender)
    //         .Include(m => m.Receiver)
    //         .ToList();

    //     return View(chatMessages);
    // }// Thêm phương thức Chat
    public IActionResult Chat()
    {
        // Kiểm tra nếu người dùng đã đăng nhập
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account"); // Chuyển hướng đến trang đăng nhập
        }

        // Lấy ID của người dùng hiện tại
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            return Unauthorized("User not authenticated.");
        }

        ViewData["AdminId"] = 1;

        // Chỉ lấy những tin nhắn mà người dùng hiện tại là người gửi hoặc người nhận
        var chatMessages = _context.ChatMessages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Where(m => (m.SenderId == userId && m.ReceiverId == 1) || (m.SenderId == 1 && m.ReceiverId == userId))
            .OrderBy(m => m.CreatedAt) // Sắp xếp tin nhắn theo thời gian
            .ToList();

        return View(chatMessages);
    }


    [HttpPost]
    public async Task<IActionResult> SendChatMessage(string messageText, int receiverId)
    {
        // Kiểm tra nếu người dùng đã đăng nhập
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account"); // Chuyển hướng đến trang đăng nhập
        }

        // Ghi log thông tin nhận được từ yêu cầu
        _logger.LogInformation("Received message: {MessageText}, ReceiverId: {ReceiverId}", messageText, receiverId);

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

        // Tạo một đối tượng ChatMessage mới
        var chatMessage = new ChatMessage
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            MessageText = messageText,
            CreatedAt = DateTime.Now
        };

        // Thêm tin nhắn vào cơ sở dữ liệu
        _context.ChatMessages.Add(chatMessage);
        _context.SaveChanges();

        // Chuyển hướng về trang chat
        return RedirectToAction("Chat");
    }



    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult DichVu()
    {
        return View();
    }
    public IActionResult TimTruong()
    {
        var schools = _context.Schools.Include(s => s.IdcategoryStabNavigation).ToList();
        return View(schools);
    }


    public IActionResult TinTuc()
    {
        // Lấy danh sách tin tức từ cơ sở dữ liệu
        var newsList = _context.News.ToList(); // Thay đổi theo yêu cầu của bạn
        return View(newsList); // Truyền danh sách vào view
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
