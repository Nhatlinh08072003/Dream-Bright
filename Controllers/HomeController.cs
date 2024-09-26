
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Models;

namespace Dream_Bridge.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult VeChungToi()
    {
        return View();
    }
    public IActionResult DuHoc()
    {
        return View();
    }
    public IActionResult Lienhe()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult DichVu()
    {
        return View();
    }
    public IActionResult TimTruong()
    {
        return View();
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
