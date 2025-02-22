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
    try
    {
        var factory = _factorySelector(country);
        var scholarship = factory.CreateScholarshipService();
        var visa = factory.CreateVisaService();
        var schoolSelection = factory.CreateSchoolSelectionService();

        // Gọi phương thức nhưng không thể lưu vào danh sách
        scholarship.ProvideScholarshipInfo();
        visa.ProvideVisaInfo();
        schoolSelection.ProvideSchoolSelectionInfo();

        var viewModel = new ConsultingViewModel
        {
            Country = country,
            Services = new List<string> // Chỉ chứa dữ liệu tĩnh hoặc nhập vào từ nơi khác
            {
                "Dịch vụ đã được thực thi."
            }
        };

        return View(viewModel);
    }
    catch (Exception ex)
    {
        return View("Error", new { errorMessage = ex.Message });
    }
}


}
