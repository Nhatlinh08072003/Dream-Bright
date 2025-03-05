using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Models;
using Dream_Bridge.Models.Main;
using Microsoft.AspNetCore.Authorization;
using Dream_Bridge.ViewModels;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Dream_Bridge.Hubs;
using Dream_Bridge.Model.Main;

namespace Dream_Bridge.Controllers;
[Route("api/chat")]
[ApiController]
public class ChatApiController : ControllerBase
{
    private readonly IChatPermissionService _chatPermissionService; //Proxy
    private readonly StudyAbroadDbContext _context; // Đảm bảo thay thế với DbContext thực tế của bạn  
    private readonly ILogger<ChatApiController> _logger;
    private readonly IHubContext<ChatHub> _hubContext;

    public ChatApiController(StudyAbroadDbContext context, IChatPermissionService chatPermissionService, ILogger<ChatApiController> logger, IHubContext<ChatHub> hubContext)
    {
        _chatPermissionService = chatPermissionService;
        _context = context;
        _logger = logger;
        _hubContext = hubContext;  // Assign IHubContext<ChatHub>
    }

    // Kiểm tra quyền truy cập trước khi vào Chat Room
    // [HttpGet("CheckChatAccess")]
    // public IActionResult CheckChatAccess(int userId)
    // {
    //     bool canAccess = _chatPermissionService.CanUserAccessChat(userId);
    //     _logger.LogInformation($"User {userId} access check: {canAccess}");

    //     if (!canAccess)
    //     {
    //         return Forbid(); // Cấm truy cập nếu không có quyền
    //     }

    //     return Ok(new { Message = "Access granted" });
    // }





    [HttpGet("messages/{userId}")]
    public IActionResult GetChatMessages(int userId, int page = 1, int pageSize = 10)
    {
        var messages = _context.ChatMessages
            .Include(cm => cm.Sender)
            .Include(cm => cm.Receiver)
            .Where(cm => cm.SenderId == userId || cm.ReceiverId == userId)
            .OrderByDescending(cm => cm.CreatedAt) // Sắp xếp theo thời gian tạo  
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Ok(messages);
    }
    // [HttpGet("messages/{userId}")]
    // public IActionResult GetChatMessages(int userId, int page = 1, int pageSize = 10)
    // {
    //     var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

    //     if (!_chatPermissionService.CanUserAccessChat(currentUserId))
    //     {
    //         return Forbid("Bạn không có quyền truy cập vào chat.");
    //     }

    //     var messages = _context.ChatMessages
    //         .Include(cm => cm.Sender)
    //         .Include(cm => cm.Receiver)
    //         .Where(cm => cm.SenderId == userId || cm.ReceiverId == userId)
    //         .OrderByDescending(cm => cm.CreatedAt)
    //         .Skip((page - 1) * pageSize)
    //         .Take(pageSize)
    //         .ToList();

    //     return Ok(messages);
    // }
    [HttpPost("send")]
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

        if (attachment != null)
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(attachment.FileName)}";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await attachment.CopyToAsync(stream);
            }
            attachmentUrl = "/uploads/" + uniqueFileName; // Đường dẫn đến tệp trong thư mục wwwroot  
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


        // Gửi tin nhắn đến client thông qua SignalR (ChatHub)
        await _hubContext.Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", chatMessage);  // Gửi tin nhắn tới client

        return Ok(new { success = true, message = chatMessage });
    }


    // public void UpdateChatPermission(int userId, bool canAccessChat)
    // {
    //     var user = _context.Users.FirstOrDefault(u => u.IdUser == userId);
    //     if (user != null && user.Role != "Staff" && user.Role != "Admin")
    //     {
    //         user.IsConsultant = canAccessChat;
    //         _context.SaveChanges();
    //     }
    // }


    public class ChatPermissionRequest
    {
        public int userId { get; set; }
        public bool canAccessChat { get; set; }
    }


    [HttpGet("users")]
    public IActionResult GetUsers()
    {
        var users = _context.Users
            .Select(u => new { u.IdUser, u.FullName })
            .ToList();

        return Ok(users);
    }

    [HttpGet("chat")]
    public IActionResult GetAllChatMessages()
    {
        var chatMessages = _context.ChatMessages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .ToList();

        return Ok(chatMessages);
    }
}