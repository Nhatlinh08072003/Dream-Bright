
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dream_Bridge.Controllers
{
    public class LoggingDichvuControllerDecorator : DichvuControllerDecorator
    {
        private readonly ILogger<LoggingDichvuControllerDecorator> _logger;

        public LoggingDichvuControllerDecorator(IDichvuController decoratedController, ILogger<LoggingDichvuControllerDecorator> logger)
            : base(decoratedController)
        {
            _logger = logger;
        }

        public override IActionResult VisaUc()
        {
            _logger.LogInformation("VisaUc action called");
            return base.VisaUc();
        }

        public override IActionResult VisaMy()
        {
            _logger.LogInformation("VisaMy action called");
            return base.VisaMy();
        }

        public override IActionResult VisaSingapore()
        {
            _logger.LogInformation("VisaSingapore action called");
            return base.VisaSingapore();
        }

        public override IActionResult VisaThuySy()
        {
            _logger.LogInformation("VisaThuySy action called");
            return base.VisaThuySy();
        }

        public override IActionResult ChiTietTruong(int id)
        {
            _logger.LogInformation($"ChiTietTruong action called with id: {id}");
            return base.ChiTietTruong(id);
        }

        public override IActionResult Chitiet(int id)
        {
            _logger.LogInformation($"Chitiet action called with id: {id}");
            return base.Chitiet(id);
        }

        public override IActionResult Error()
        {
            _logger.LogInformation("Error action called");
            return base.Error();
        }
    }
}
