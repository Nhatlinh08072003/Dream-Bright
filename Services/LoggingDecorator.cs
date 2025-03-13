public class LoggingDecorator : IDichvuService
{
    private readonly DichvuService _decoratedService;
    private readonly ILogger<LoggingDecorator> _logger;

    public LoggingDecorator(DichvuService decoratedService, ILogger<LoggingDecorator> logger)
    {
        _decoratedService = decoratedService;
        _logger = logger;
    }

    public void Log(string message)
    {
        _logger.LogInformation("LoggingDecorator: " + message);
        _decoratedService.Log(message);
    }
}
