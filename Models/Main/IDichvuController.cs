
using Microsoft.AspNetCore.Mvc;

namespace Dream_Bridge.Controllers
{
    public interface IDichvuController
    {
        IActionResult VisaUc();
        IActionResult VisaMy();
        IActionResult VisaSingapore();
        IActionResult VisaThuySy();
        IActionResult ChiTietTruong(int id);
        IActionResult Chitiet(int id);
        IActionResult Error();
    }
}
