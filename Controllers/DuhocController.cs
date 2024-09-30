using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Models;

namespace Dream_Bridge.Controllers;
public class DuhocController : Controller
{
    private readonly ILogger<DuhocController> _logger;

    public DuhocController(ILogger<DuhocController> logger)
    {
        _logger = logger;
    }

    public IActionResult Uc()
    {
        return View();
    }
    public IActionResult My()
    {
        return View();
    }
    public IActionResult Singapore()
    {
        return View();
    }
    public IActionResult ThuySy()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}