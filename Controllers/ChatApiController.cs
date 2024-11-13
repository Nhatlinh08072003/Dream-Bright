using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Models;
using Dream_Bridge.Models.Main;
using Microsoft.AspNetCore.Authorization;
using Dream_Bridge.ViewModels;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Dream_Bridge.Controllers;
[Route("api/chat")]  
[ApiController]  
public class ChatApiController : ControllerBase  
{  
    private readonly StudyAbroadDbContext _context; // Đảm bảo thay thế với DbContext thực tế của bạn  
    private readonly ILogger<ChatApiController> _logger;  

    public ChatApiController(StudyAbroadDbContext context, ILogger<ChatApiController> logger)  
    {  
        _context = context;  
        _logger = logger;  
    }  

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

        return Ok(new { success = true, message = chatMessage });  
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