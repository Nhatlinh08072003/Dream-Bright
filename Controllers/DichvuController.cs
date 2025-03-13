using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Models;
using Dream_Bridge.Models.Main;
using Dream_Bridge.Services;
using DreamBright.Models;

namespace Dream_Bridge.Controllers;
public class DichvuController : Controller
{
    private readonly ILogger<DichvuController> _logger;
    private readonly StudyAbroadDbContext _context;
    private readonly IDichvuService _dichvuService;

    public DichvuController(ILogger<DichvuController> logger, StudyAbroadDbContext context, IDichvuService dichvuService)
    {
        _logger = logger;
        _context = context;
        _dichvuService = dichvuService;
    }

    public IActionResult VisaUc()
    {
        _dichvuService.Log("VisaUc action called");
        return View();
    }
    public IActionResult VisaMy()
    {
        _dichvuService.Log("VisaMy action called");
        return View();
    }
    public IActionResult VisaSingapore()
    {
        _dichvuService.Log("VisaSingapore action called");
        return View();
    }
    public IActionResult VisaThuySy()
    {
        _dichvuService.Log("VisaThuySy action called");
        return View();
    }

    public IActionResult ChiTietTruong(int id)
    {
        _dichvuService.Log($"ChiTietTruong action called with id: {id}");
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
        _dichvuService.Log($"Chitiet action called with id: {id}");
        // Tìm tin tức theo IdNews
        var newsItem = _context.News.FirstOrDefault(n => n.IdNews == id);
        if (newsItem == null)
        {
            return NotFound("Không tìm thấy tin tức.");
        }

        return View(newsItem);
    }


    public IActionResult DemoDecoratorPattern()
    {
        _dichvuService.Log("DemoDecoratorPattern action called");
        var demoModel = new DemoDecoratorPatternViewModel
        {
            Message = "Decorator pattern demo executed. Check logs for details."
        };
        return View(demoModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}