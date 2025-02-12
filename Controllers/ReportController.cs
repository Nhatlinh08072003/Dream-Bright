using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dream_Bridge.Models.Main;

[Route("api/reports")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly StudyAbroadDbContext _dbContext;

    public ReportController(StudyAbroadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("export/{type}")]
    public IActionResult ExportReport(string type)
    {
        ReportTemplate<User> report = type.ToLower() switch
        {
            "pdf" => new PDFReport(_dbContext),
            "csv" => new CSVReport(_dbContext),
            "json" => new JSONReport(_dbContext),
            _ => null
        };

        if (report == null)
        {
            return BadRequest("Invalid report type. Use 'pdf', 'csv', or 'json'.");
        }

        report.GenerateReport();
        return Ok($"Report {type.ToUpper()} generated successfully!");
    }
}
