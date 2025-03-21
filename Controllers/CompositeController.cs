using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Models.Composite;

namespace Dream_Bridge.Controllers
{
    public class CompositeController : Controller
    {
        public IActionResult Index()
        {
            var visaConsulting = new SingleService("Tư vấn Visa");
            var scholarshipSupport = new SingleService("Hỗ trợ Học bổng");

            var studyAbroadServices = new ServiceGroup("Dịch vụ Du học");
            studyAbroadServices.AddService(visaConsulting);
            studyAbroadServices.AddService(scholarshipSupport);

            return View(studyAbroadServices);
        }
    }
}
