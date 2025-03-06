using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Security.Claims;

public class LogHub : Hub
{
    private readonly LoggerSingleton _logger;

    public LogHub(LoggerSingleton logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            // Gửi log của user khi kết nối SignalR
            await Clients.Caller.SendAsync("ReceiveLog", _logger.Logs);
        }
        await base.OnConnectedAsync();
    }

    public async Task SendLog(string level, string message)
    {
        _logger.AddLog(level, message);
        await Clients.All.SendAsync("ReceiveLog", _logger.Logs); // ✅ Trả về _logger.Logs
    }
}
