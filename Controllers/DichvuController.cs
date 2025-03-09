using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Models;
using Dream_Bridge.Models.Main;
using Dream_Bridge.Services;

namespace Dream_Bridge.Controllers;
public class DichvuController : Controller, IDichvuController
{
    private readonly IDichvuController _decoratedController;
    private readonly ILogger<DichvuController> _logger;
    private readonly StudyAbroadDbContext _context;

    public DichvuController(IDichvuController decoratedController, ILogger<DichvuController> logger, StudyAbroadDbContext context)
    {
        _decoratedController = decoratedController;
        _logger = logger;
        _context = context;
    }

    public IActionResult VisaUc()
    {
        _logger.LogInformation("VisaUc action called");
        return _decoratedController.VisaUc();
    }
    public IActionResult VisaMy()
    {
        _logger.LogInformation("VisaMy action called");
        return _decoratedController.VisaMy();
    }
    public IActionResult VisaSingapore()
    {
        _logger.LogInformation("VisaSingapore action called");
        return _decoratedController.VisaSingapore();
    }
    public IActionResult VisaThuySy()
    {
        _logger.LogInformation("VisaThuySy action called");
        return _decoratedController.VisaThuySy();
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