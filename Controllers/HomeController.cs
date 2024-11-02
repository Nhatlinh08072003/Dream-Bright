
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
        var schools = _context.Schools.ToList(); // Lấy danh sách trường từ cơ sở dữ liệu
        return View(schools);
    }
    [HttpPost]
    public IActionResult SearchSchools(string nation, string city, string educationLevel)
    {
        // Lọc danh sách trường dựa trên các tiêu chí tìm kiếm
        var filteredSchools = _context.Schools
            .Where(s => (string.IsNullOrEmpty(nation) || s.Nation == nation) &&
                        (string.IsNullOrEmpty(city) || s.StateCity == city) &&
                        (string.IsNullOrEmpty(educationLevel) || s.Level == educationLevel))
            .ToList();

        return View("TimTruong", filteredSchools); // Trả về view với kết quả đã lọc
    }

    public IActionResult TinTuc()
    {
        // Lấy danh sách tin tức từ cơ sở dữ liệu
        var newsList = _context.News.ToList(); // Thay đổi theo yêu cầu của bạn
        return View(newsList); // Truyền danh sách vào view
    }

    [HttpGet("api/schools")]
    public IActionResult GetSchools(string country, string city, string educationLevel)
    {

        var schools = _context.Schools
            .Where(s => s.Nation == country && s.StateCity == city && s.Level == educationLevel)
            .ToList();

        if (schools == null || !schools.Any())
        {
            return NotFound("No schools found.");
        }

        return Ok(schools);
    }




    // [HttpGet]
    // [Route("schools")]
    // public async Task<IActionResult> GetSchools(string country, string city, string educationLevel)
    // {
    //     var schools = await _context.Schools
    //         .Where(s => s.Nation == country && s.StateCity == city && s.Level == educationLevel)
    //         .ToListAsync();

    //     if (schools == null || !schools.Any())
    //     {
    //         return NotFound(new { message = "No schools found for the given criteria." });
    //     }

    //     return Ok(schools);
    // }


    // Phương thức lấy danh sách quốc gia
    [HttpGet("api/nations")]
    public IActionResult GetNations()
    {
        try
        {
            var nations = _context.Schools
                .Select(s => s.Nation)
                .Distinct()
                .ToList();

            if (nations == null || !nations.Any())
            {
                return NotFound("No nations found.");
            }

            return Ok(nations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching nations.");
            return StatusCode(500, "Internal server error.");
        }
    }

    // Phương thức lấy danh sách thành phố theo quốc gia
    [HttpGet("api/cities/{nation}")]
    public IActionResult GetCities(string nation)
    {
        var cities = _context.Schools
            .Where(s => s.Nation == nation)
            .Select(s => s.StateCity)
            .Distinct()
            .ToList();

        return Ok(cities);
    }


    [HttpGet("api/education-levels/{nation}")]
    public IActionResult GetEducationLevels(string nation)
    {
        var levels = _context.Schools
            .Where(s => s.Nation == nation)
            .Select(s => s.Level)
            .Distinct()
            .ToList();

        return Ok(levels);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
