using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Dream_Bridge.Models;
using Dream_Bridge.Models.Main;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Dream_Bright.Models.Main;
using System.Text;

namespace MyProject.Controllers
{
    public class ReportController : Controller
    {
        private readonly StudyAbroadDbContext _dbContext;

        public ReportController(StudyAbroadDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("export/{type}")]
        public IActionResult ExportReport(string type)
        {
            Models.ReportTemplate<User> report = type.ToLower() switch
            {
                "pdf" => new Models.PDFReport(_dbContext),
                "csv" => new Models.CSVReport(_dbContext),
                "json" => new Models.JSONReport(_dbContext),
                _ => null
            };

            if (report == null)
            {
                return BadRequest("Invalid report type. Use 'pdf', 'csv', or 'json'.");
            }

            string filePath = report.GenerateReport();
            string fileUrl = $"/reports/{Path.GetFileName(filePath)}";

            TempData["Message"] = $"Report {type.ToUpper()} generated successfully!";
            TempData["FileUrl"] = fileUrl;

            return RedirectToAction("Index");
        }
    }
}
