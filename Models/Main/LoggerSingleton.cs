using Microsoft.Extensions.Logging;

public sealed class LoggerSingleton
{
    private static readonly Lazy<LoggerSingleton> _instance =
        new Lazy<LoggerSingleton>(() => new LoggerSingleton());

    private static int _instanceCount = 0; // Đếm số lần tạo instance

    private LoggerSingleton()
    {
        _instanceCount++;
        Console.WriteLine($"🔥 LoggerSingleton instance created: {_instanceCount}");
    }

    public static LoggerSingleton Instance => _instance.Value;

    public void Log(string message)
    {
        Console.WriteLine($"[LOG]: {message}");
    }
}

