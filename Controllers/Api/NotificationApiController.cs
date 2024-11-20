using Dream_Bridge.Models.Main;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Dream_Bridge.Hubs;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly StudyAbroadDbContext _context;
    private readonly IHubContext<ChatHub> _hubContext; // SignalR Hub

    public NotificationsController(StudyAbroadDbContext context, IHubContext<ChatHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    // Lấy danh sách thông báo chưa đọc
    [HttpGet]
    public async Task<IActionResult> GetNotifications()
    {
        var notifications = await _context.Notifications
            .Where(n => !n.IsRead) // Lọc thông báo chưa đọc    
            .OrderByDescending(n => n.CreatedAt)
            .Take(1) // Giới hạn số thông báo nếu cần
            .ToListAsync();

        return Ok(notifications);
    }

    // Tạo thông báo mới
    [HttpPost]
    public async Task<IActionResult> CreateNotification([FromBody] Notification notification)
    {
        if (notification == null)
        {
            return BadRequest("Thông báo không hợp lệ");
        }

        // Thêm thông báo mới vào cơ sở dữ liệu
        notification.CreatedAt = DateTime.Now; // Đặt thời gian tạo thông báo
        notification.IsRead = false; // Đánh dấu là chưa đọc

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        // Gửi thông báo cho tất cả người dùng qua SignalR
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);

        return Ok(notification); // Trả về thông báo vừa tạo
    }

    // Đánh dấu thông báo là đã đọc
    [HttpPut("{id}")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var notification = await _context.Notifications.FindAsync(id);

        if (notification == null)
        {
            return NotFound("Thông báo không tồn tại");
        }

        notification.IsRead = true;
        await _context.SaveChangesAsync();

        return Ok(notification); // Trả về thông báo đã được đánh dấu là đã đọc
    }
}
