public class DichvuService : IDichvuService
{
    private readonly ILogger<DichvuService> _logger;

    public DichvuService(ILogger<DichvuService> logger)
    {
        _logger = logger;
    }

    public void Log(string message)
    {
        _logger.LogInformation(message);
    }
}
