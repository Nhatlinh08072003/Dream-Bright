// Cải tiến VNPayService với logging
using Dream_Bright.Services.Logging;
namespace Dream_Bright.Services.PaymentGateways
{
    public class VNPayService
    {
        private readonly ITransactionLogger _logger;

        public VNPayService(ITransactionLogger logger)
        {
            _logger = logger;
        }

        public void Pay(decimal amount)
        {
            _logger.LogTransaction($"Initiating VNPay payment for {amount} VND");
            // Simulate API call
            Thread.Sleep(1000);
            _logger.LogTransaction($"VNPay payment processed for {amount} VND");
        }
    }
}