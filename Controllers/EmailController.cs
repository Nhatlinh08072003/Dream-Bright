using Dream_Bridge.Models.Main;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly EmailService _emailService;

    public EmailController(StudyAbroadDbContext dbContext) // Nhận dbContext qua constructor
    {
        _emailService = new EmailService(dbContext); // Truyền dbContext vào EmailService
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendEmail([FromBody] EmailRequest emailRequest) // Đổi thành Task<IActionResult>
    {
        if (string.IsNullOrEmpty(emailRequest.ToEmail) ||
            string.IsNullOrEmpty(emailRequest.Subject) ||
            string.IsNullOrEmpty(emailRequest.Body))
        {
            return BadRequest("All fields are required.");
        }

        try
        {
            await _emailService.SendEmailAsync(emailRequest.ToEmail, emailRequest.Subject, emailRequest.Body); // Chờ kết quả từ SendEmailAsync
            return Ok("Email sent successfully!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] EmailRequest emailRequest) // Đổi thành Task<IActionResult>
    {
        if (string.IsNullOrEmpty(emailRequest.ToEmail) ||
            string.IsNullOrEmpty(emailRequest.Subject) ||
            string.IsNullOrEmpty(emailRequest.Body))
        {
            return BadRequest("All fields are required.");
        }

        try
        {
            await _emailService.SendEmailAsync(emailRequest.ToEmail, emailRequest.Subject, emailRequest.Body); // Chờ kết quả từ SendEmailAsync
            return Ok("Registration email sent successfully!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
}
