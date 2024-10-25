using Dream_Bridge.Models.Main;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly EmailService _emailService;
    private readonly StudyAbroadDbContext _context;

    public EmailController(StudyAbroadDbContext dbContext, StudyAbroadDbContext context) // Nhận dbContext qua constructor
    {
        _emailService = new EmailService(dbContext); // Truyền dbContext vào EmailService
        _context = context;
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

    // [HttpPost("register")]
    // public async Task<IActionResult> Register([FromBody] EmailRequest emailRequest) // Đổi thành Task<IActionResult>
    // {
    //     if (string.IsNullOrEmpty(emailRequest.ToEmail) ||
    //         string.IsNullOrEmpty(emailRequest.Subject) ||
    //         string.IsNullOrEmpty(emailRequest.Body))
    //     {
    //         return BadRequest("All fields are required.");
    //     }

    //     try
    //     {
    //         await _emailService.SendEmailAsync(emailRequest.ToEmail, emailRequest.Subject, emailRequest.Body); // Chờ kết quả từ SendEmailAsync
    //         return Ok("Registration email sent successfully!");
    //     }
    //     catch (Exception ex)
    //     {
    //         return StatusCode(500, "Internal server error: " + ex.Message);
    //     }
    // }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequest registrationRequest)
    {
        // Validate the incoming request
        if (string.IsNullOrEmpty(registrationRequest.FullName) ||
            string.IsNullOrEmpty(registrationRequest.Email) ||
            string.IsNullOrEmpty(registrationRequest.PhoneNumber) ||
            string.IsNullOrEmpty(registrationRequest.ConsultingType) ||
            string.IsNullOrEmpty(registrationRequest.ContentConsulting))
        {
            return BadRequest("All fields are required.");
        }

        // Save to the database
        var consultingRegistration = new ConsultingRegistration
        {
            FullName = registrationRequest.FullName,
            Email = registrationRequest.Email,
            PhoneNumber = registrationRequest.PhoneNumber,
            ConsultingType = registrationRequest.ConsultingType,
            ContentConsulting = registrationRequest.ContentConsulting,
            Note = registrationRequest.Note,
            Status = "Chưa tư vấn",
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            // Save registration to the database
            await _context.ConsultingRegistrations.AddAsync(consultingRegistration);
            _context.SaveChanges();

            // Send the email
            await _emailService.SendEmailAsync(registrationRequest.Email, $"Đăng ký tư vấn từ {registrationRequest.FullName}", $"Tên: {registrationRequest.FullName}\nSố điện thoại: {registrationRequest.PhoneNumber}\nDịch vụ: {registrationRequest.ConsultingType}\nQuốc gia: {registrationRequest.ContentConsulting}\nNội dung: {registrationRequest.Note}");

            return Ok("Registration and email sent successfully!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
}
