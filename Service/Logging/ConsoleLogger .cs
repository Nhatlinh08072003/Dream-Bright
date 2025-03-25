namespace Dream_Bright.Services.Logging
{
    public class ConsoleLogger : ITransactionLogger
    {
        public void LogTransaction(string message)
        {
            Console.WriteLine($"[Transaction Log] {DateTime.Now}: {message}");
        }
    }
}