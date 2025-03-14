using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Dream_Bridge.Models;
using Dream_Bridge.Models.Main;

using Dream_Bridge.Builders;
using Dream_Bright.Models.Main;
using DreamBright.Models;

namespace Dream_Bridge.Controllers;

public class HomeController : Controller
{
    private readonly StudyAbroadDbContext _context;
    // private readonly ILogger<HomeController> _logger;
    private readonly SchoolQueryBuilder _queryBuilder;
    private readonly LoggerSingleton _logger;

    public HomeController(LoggerSingleton logger, StudyAbroadDbContext context)
    {
        _logger = logger;
        _context = context;
        _queryBuilder = new SchoolQueryBuilder(_context.Schools.AsQueryable());
    }

    /*************  ✨ Codeium Command ⭐  *************/
    /// <summary>
    /// Handles the request for the home page and logs the access.
    /// </summary>
    /// <returns>The view for the home page.</returns>

    /******  82e744c0-a362-4b87-93fc-eedb0b94f0bc  *******/
    // public IActionResult Index()
    // {
    //     var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Guest"; // Lấy userId hoặc "Guest"
    //     _logger.AddLog("INFO", $"User {userId} đã truy cập trang Home."); // Ghi log

    //     return View();
    // }
    // public IActionResult Index()
    //     {
    //         var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Guest";
    //         _logger.AddLog("INFO", $"User {userId} đã truy cập trang Home.");

    //         // Lấy danh sách thông báo từ database
    //         var notifications = _context.Notifications
    //             .OrderByDescending(n => n.CreatedAt)
    //             .Take(10) // Giới hạn 10 thông báo mới nhất
    //             .Select(n => new NotificationViewModel
    //             {
    //                 UserId = n.UserId ?? 0,
    //                 Title = n.Title,
    //                 Message = n.Message,
    //                 Type = n.Type
    //             })
    //             .ToList();

    //         var viewModel = new IndexViewModel
    //         {
    //             Notifications = notifications
    //         };

    //         return View(viewModel);
    //     }
    public IActionResult Index()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        int? currentUserId = null;

        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int parsedUserId))
        {
            currentUserId = parsedUserId;
            _logger.AddLog("INFO", $"User {currentUserId} đã truy cập trang Home.");
        }
        else
        {
            _logger.AddLog("INFO", "Người dùng chưa đăng nhập đã truy cập trang Home.");
        }

        var notifications = new List<NotificationViewModel>(); // Khởi tạo mặc định là danh sách rỗng
        if (currentUserId.HasValue)
        {
            notifications = _context.Notifications
                .Where(n => n.UserId == currentUserId.Value)
                .OrderByDescending(n => n.CreatedAt)
                .Take(10)
                .Select(n => new NotificationViewModel
                {
                    UserId = n.UserId ?? 0,
                    Title = n.Title,
                    Message = n.Message,
                    Type = n.Type
                })
                .ToList();
        }

        var viewModel = new IndexViewModel
        {
            Notifications = notifications
        };

        return View(viewModel);
    }
    public IActionResult VeChungToi() => View();
    public IActionResult DuHoc() => View();
    public IActionResult DichVu() => View();
    public IActionResult ChatBot() => View();

    [HttpPost]
    public IActionResult ChatBot([FromBody] ChatRequest data)
    {
        if (string.IsNullOrEmpty(data.Message))
            return Json(new { response = "Bạn chưa nhập tin nhắn!" });

        var chatbot = new ChatbotContext();
        return Json(new { response = chatbot.GetResponse(data.Message) });
    }

    public IActionResult TinTuc()
    {
        var news = _context.News.ToList();
        return View(news);
    }

    public IActionResult TimTruong(string country, string city, string level, decimal? maxTuition)
    {
        // Demonstrating the usage of the Builder pattern
        var schools = _queryBuilder
            .FilterByCountry(country)
            .FilterByCity(city)
            .FilterByLevel(level)
            .FilterByMaxTuition(maxTuition)
            .Build()
            .OrderBy(s => s.AverageTuition)
            .ToList();

        var maxTuitionFee = _context.Schools.Max(s => s.AverageTuition);

        ViewBag.MaxTuition = maxTuitionFee;

        return View(schools);
    }

    [HttpPost("api/schools")]
    public IActionResult FilterSchools([FromBody] FilterViewModel filter)
    {
        var filteredSchools = _queryBuilder
            .FilterByCountry(filter.Country)
            .FilterByCity(filter.City)
            .FilterByLevel(filter.EducationLevel)
            .FilterByMaxTuition(filter.MaxTuition) // Chỉ truyền MaxTuition
            .Build()
            .ToList();

        return Json(filteredSchools);
    }


    [HttpGet("api/nations")]
    public IActionResult GetNations()
    {
        return Json(_context.Schools.Select(s => s.Nation).Distinct().ToList());
    }

    [HttpGet("api/cities/{nation}")]
    public IActionResult GetCities(string nation)
    {
        return Json(_context.Schools.Where(s => s.Nation == nation).Select(s => s.StateCity).Distinct().ToList());
    }

    [HttpGet("api/education-levels/{nation}")]
    public IActionResult GetEducationLevels(string nation)
    {
        return Json(_context.Schools.Where(s => s.Nation == nation).Select(s => s.Level).Distinct().ToList());
    }

    [HttpGet("api/max-tuition")]
    public IActionResult GetMaxTuition()
    {
        var maxTuition = _context.Schools.Max(s => s.AverageTuition);
        return Json(maxTuition);
    }

    [Authorize]
    public IActionResult Chat()
    {
        if (User.IsInRole("Admin") || User.IsInRole("Staff"))
            return RedirectToAction("qlchat", "Admin");

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized("User not authenticated.");

        ViewData["AdminId"] = 1;
        var chatMessages = _context.ChatMessages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Where(m => (m.SenderId == userId && m.ReceiverId == 1) || (m.SenderId == 1 && m.ReceiverId == userId))
            .OrderBy(m => m.CreatedAt)
            .ToList();

        return View(chatMessages);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SendChatMessage(string messageText, int receiverId, IFormFile? attachment)
    {
        if (string.IsNullOrEmpty(messageText))
            return BadRequest("Message text cannot be empty.");

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int senderId))
            return Unauthorized("User not authenticated.");

        var receiver = await _context.Users.FindAsync(receiverId);
        if (receiver == null)
            return NotFound("Receiver not found.");

        string attachmentUrl = null;
        if (attachment != null)
        {
            var uploadsFolder = Path.Combine("wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder);
            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(attachment.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await attachment.CopyToAsync(stream);
            attachmentUrl = $"/uploads/{uniqueFileName}";
        }

        var chatMessage = new ChatMessage
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            MessageText = messageText,
            CreatedAt = DateTime.Now,
            AttachmentUrl = attachmentUrl
        };

        _context.ChatMessages.Add(chatMessage);
        await _context.SaveChangesAsync();
        return RedirectToAction("Chat");
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> DeleteChatMessage(int messageId)
    {
        var message = await _context.ChatMessages.FindAsync(messageId);
        if (message == null)
            return NotFound("Message not found.");

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId) || message.SenderId != userId)
            return Unauthorized("You can only delete your own messages.");

        _context.ChatMessages.Remove(message);
        await _context.SaveChangesAsync();
        return RedirectToAction("Chat");
    }

    public IActionResult DemoBuilderPattern()
    {
        var schools = _queryBuilder
            .FilterByCountry("USA")
            .FilterByCity("New York")
            .FilterByLevel("Undergraduate")
            .FilterByMaxTuition(30000)
            .Build()
            .ToList();

        return Json(schools);
    }

    /// <summary>
    ///     Renders the error page with the request ID.
    /// </summary>
    /// <returns>The error view.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
