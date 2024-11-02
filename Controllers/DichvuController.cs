using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Models;
using Dream_Bridge.Models.Main;

namespace Dream_Bridge.Controllers;
public class DichvuController : Controller
{
    private readonly ILogger<DichvuController> _logger;
    private readonly StudyAbroadDbContext _context;

    public DichvuController(ILogger<DichvuController> logger, StudyAbroadDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult VisaUc()
    {
        return View();
    }
    public IActionResult VisaMy()
    {
        return View();
    }
    public IActionResult VisaSingapore()
    {
        return View();
    }
    public IActionResult VisaThuySy()
    {
        return View();
    }

    public IActionResult ChiTietTruong(int id)
    {
        var school = _context.Schools.Find(id);
        if (school == null)
        {
            return NotFound();
        }
        return View(school);
    }

    // Action chi tiết tin tức
    [HttpGet("News/Detail")]
    public IActionResult Chitiet(int id)
    {
        // Tìm tin tức theo IdNews
        var newsItem = _context.News.FirstOrDefault(n => n.IdNews == id);
        if (newsItem == null)
        {
            return NotFound("Không tìm thấy tin tức.");
        }

        return View(newsItem);
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}