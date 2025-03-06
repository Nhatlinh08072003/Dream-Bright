using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

public class LoggerSingleton
{
    public int InstanceId { get; } = Guid.NewGuid().GetHashCode();
    private readonly List<string> _logs = new();
    private readonly object _lock = new();
    public event Action<List<string>>? OnLogUpdated;

    public IReadOnlyList<string> Logs => _logs.AsReadOnly();

    public LoggerSingleton() // ✅ Constructor công khai để DI có thể khởi tạo
    {
        Console.WriteLine($"📌 Logger Instance Created: {InstanceId}");
    }

    public void AddLog(string level, string message)
    {
        lock (_lock)
        {
            string logEntry = $"{DateTime.Now:HH:mm:ss} [{level}] - {message}";
            _logs.Add(logEntry);

            if (_logs.Count > 50) _logs.RemoveAt(0);

            Console.WriteLine($"✅ LOG ADDED: {logEntry}");

            OnLogUpdated?.Invoke(_logs);
        }
    }
}
