using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Dream_Bridge.Models;
using Dream_Bridge.Models.Main;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Dream_Bright.Models.Main;
[Route("api/logs")]
[ApiController]
public class LogController : Controller
{
    private readonly LoggerSingleton _logger;

    public LogController(LoggerSingleton logger)
    {
        _logger = logger;
    }
    [HttpGet("instance")]
    public IActionResult GetInstanceId()
    {
        return Ok(_logger.InstanceId);
    }
    [HttpGet]
    public IActionResult GetLogs()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(); // Nếu chưa đăng nhập, từ chối truy cập

        return Ok(_logger.Logs); // Trả về log của user hiện tại
    }


    [HttpPost]
    public IActionResult AddLog(string level, string message)
    {
        return Ok(new { success = true });
    }
    [HttpGet("/log")]
    public IActionResult Index()
    {
        return View();
    }

}
