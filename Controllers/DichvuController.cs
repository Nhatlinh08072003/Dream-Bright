using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Models;

namespace Dream_Bridge.Controllers;
public class DichvuController : Controller
{
    private readonly ILogger<DichvuController> _logger;

    public DichvuController(ILogger<DichvuController> logger)
    {
        _logger = logger;
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
    public IActionResult ChiTiet()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}