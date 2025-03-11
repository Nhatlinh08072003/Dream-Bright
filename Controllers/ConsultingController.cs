using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DreamBright.Models;
using DreamBright.Services.Factory;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace DreamBright.Controllers
{
    public class ConsultingController : Controller
    {
        private readonly IStudyAbroadFactorySelector _factorySelector;
        private readonly ILogger<ConsultingController> _logger;

        // Chỉ giữ lại một constructor duy nhất
        public ConsultingController(
            IStudyAbroadFactorySelector factorySelector, 
            ILogger<ConsultingController> logger)
        {
            _factorySelector = factorySelector ?? throw new ArgumentNullException(nameof(factorySelector));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public IActionResult GetServices(string country)
        {
            try
            {
                if (string.IsNullOrEmpty(country))
                {
                    _logger.LogWarning("Country parameter is null or empty");
                    return RedirectToAction("DuHoc", "Home");
                }

                // Xử lý chuỗi country
                country = country.Trim();
                if (country.Contains("Mỹ", StringComparison.OrdinalIgnoreCase))
                    country = "US";
                else if (country.Contains("Anh", StringComparison.OrdinalIgnoreCase))
                    country = "UK";

                _logger.LogInformation($"Processing request for country code: {country}");

                _logger.LogInformation($"Getting services for country: {country}");
                
                var factory = _factorySelector.GetFactory(country);
                if (factory == null)
                {
                    _logger.LogError($"Factory not found for country: {country}");
                    throw new InvalidOperationException($"Không thể tạo factory cho quốc gia: {country}");
                }

                var scholarship = factory.CreateScholarshipService();
                var visa = factory.CreateVisaService();
                var schoolSelection = factory.CreateSchoolSelectionService();

                var viewModel = new DreamBright.Models.ConsultingViewModel
                {
                    Country = country,
                    ScholarshipInfo = scholarship.ProvideScholarshipInfo(),
                    VisaInfo = visa.ProvideVisaInfo(),
                    SchoolSelectionInfo = schoolSelection.ProvideSchoolSelectionInfo(),
                    Services = new List<string>
                    {
                        "Dịch vụ học bổng",
                        "Dịch vụ visa",
                        "Dịch vụ chọn trường"
                    }
                };

                _logger.LogInformation($"Returning view for country: {country}");
                
                return country?.ToUpper() switch
                {
                    "US" => View("USServices", viewModel),
                    "UK" => View("UKServices", viewModel),
                    _ => View("Error", new ErrorViewModel { 
                        ErrorMessage = "Không hỗ trợ quốc gia này",
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                    })
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing request for country {country}: {ex}");
                return View("Error", new ErrorViewModel { 
                    ErrorMessage = $"Có lỗi xảy ra khi xử lý yêu cầu: {ex.Message}",
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }
    }
}
