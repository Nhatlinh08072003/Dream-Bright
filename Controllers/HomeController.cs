using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Models;
using Dream_Bridge.Models.Main;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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
        var logger1 = LoggerSingleton.Instance;
        var logger2 = LoggerSingleton.Instance;

        _logger.LogInformation("📌 Log từ instance 1");
        _logger.LogInformation("📌 Log từ instance 2");
        _logger.LogInformation($"🧐 Cùng instance? {ReferenceEquals(logger1, logger2)}");

        return View();
    }

    public IActionResult VeChungToi() => View();
    public IActionResult DuHoc() => View();
    public IActionResult DichVu() => View();

    public IActionResult TinTuc()
    {
        var news = _context.News.ToList();
        return View(news);
    }

    public IActionResult TimTruong()
    {
        var schools = _context.Schools.ToList();
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

        return Json(filteredSchools);
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

    [Authorize]
    public IActionResult Chat()
    {
        if (User.IsInRole("Admin") || User.IsInRole("Staff"))
        {
            return RedirectToAction("qlchat", "Admin");
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            return Unauthorized("User not authenticated.");
        }

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
        {
            return BadRequest("Message text cannot be empty.");
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int senderId))
        {
            return Unauthorized("User not authenticated.");
        }

        var receiver = await _context.Users.FindAsync(receiverId);
        if (receiver == null)
        {
            return NotFound("Receiver not found.");
        }

        string attachmentUrl = null;
        if (attachment != null)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            Directory.CreateDirectory(uploadsFolder);
            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(attachment.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await attachment.CopyToAsync(stream);
            }
            attachmentUrl = "/uploads/" + uniqueFileName;
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
        {
            return NotFound("Message not found.");
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId) || message.SenderId != userId)
        {
            return Unauthorized("You can only delete your own messages.");
        }

        _context.ChatMessages.Remove(message);
        await _context.SaveChangesAsync();

        return RedirectToAction("Chat");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

