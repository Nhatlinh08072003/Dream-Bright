using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Services.Factories;
using Dream_Bridge.Services.Adapters;
using Dream_Bridge.Services.Logging;
using Dream_Bridge.Services.Repositories;

namespace Dream_Bridge.Controllers
{
    public class TestController : Controller
    {
        private readonly IServiceFactory _serviceFactory;

        public TestController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public IActionResult Index()
        {
            var apiAdapter = _serviceFactory.CreateApiAdapter();
            var loggingService = _serviceFactory.CreateLoggingService();
            var repository = _serviceFactory.CreateRepository<object>();

            return Content(
                $"ApiAdapter: {apiAdapter.GetType().Name}, " +
                $"LoggingService: {loggingService.GetType().Name}, " +
                $"Repository: {repository.GetType().Name}"
            );
        }
    }
}
