using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Models;
using Dream_Bridge.Models.Main;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
namespace Dream_Bridge.Controllers;
public class ConsultingController : Controller
{
    private readonly Func<string, IStudyAbroadFactory> _factorySelector;

    public ConsultingController(Func<string, IStudyAbroadFactory> factorySelector)
    {
        _factorySelector = factorySelector;
    }

    public IActionResult GetServices(string country)
    {
        var factory = _factorySelector(country);
        var scholarship = factory.CreateScholarshipService();
        var visa = factory.CreateVisaService();
        var schoolSelection = factory.CreateSchoolSelectionService();

        scholarship.ProvideScholarshipInfo();
        visa.ProvideVisaInfo();
        schoolSelection.ProvideSchoolSelectionInfo();

        return Ok($"Dịch vụ tư vấn cho {country} đã được gọi!");
    }
}
