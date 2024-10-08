using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly EmailService _emailService;

    public EmailController()
    {
        _emailService = new EmailService();
    }

    [HttpPost("send")]
    public IActionResult SendEmail([FromBody] EmailRequest emailRequest)
    {
        if (string.IsNullOrEmpty(emailRequest.ToEmail) ||
            string.IsNullOrEmpty(emailRequest.Subject) ||
            string.IsNullOrEmpty(emailRequest.Body))
        {
            return BadRequest("All fields are required.");
        }

        _emailService.SendEmail(emailRequest.ToEmail, emailRequest.Subject, emailRequest.Body);
        return Ok("Email sent successfully!");
    }


[HttpPost("register")]
public IActionResult Register([FromBody] EmailRequest emailRequest)
{
    if (string.IsNullOrEmpty(emailRequest.ToEmail) ||
        string.IsNullOrEmpty(emailRequest.Subject) ||
        string.IsNullOrEmpty(emailRequest.Body))
    {
        return BadRequest("All fields are required.");
    }

    try
    {
        _emailService.SendEmail(emailRequest.ToEmail, emailRequest.Subject, emailRequest.Body);
        return Ok("Registration email sent successfully!");
    }
    catch (Exception ex)
    {
        return StatusCode(500, "Internal server error: " + ex.Message);
    }
}
}

