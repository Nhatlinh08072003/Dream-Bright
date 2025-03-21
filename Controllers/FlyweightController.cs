using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Models.Flyweight;

namespace Dream_Bridge.Controllers
{
    public class FlyweightController : Controller
    {
        public IActionResult Index()
        {
            var universities = new List<University>
            {
                UniversityFactory.GetUniversity("Harvard", "USA"),
                UniversityFactory.GetUniversity("Oxford", "UK"),
                UniversityFactory.GetUniversity("Harvard", "USA") // Trùng lặp, dùng object đã có
            };

            return View(universities);
        }
    }
}
