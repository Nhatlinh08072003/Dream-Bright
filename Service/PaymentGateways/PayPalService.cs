// Cải tiến PayPalService với logging
using Dream_Bright.Services.Logging;
namespace Dream_Bright.Services.PaymentGateways
{
    public class PayPalService
    {
        private readonly ITransactionLogger _logger;

        public PayPalService(ITransactionLogger logger)
        {
            _logger = logger;
        }

        public void MakePayment(decimal amount)
        {
            _logger.LogTransaction($"Initiating PayPal payment for {amount} USD");
            // Simulate API call
            Thread.Sleep(1000);
            _logger.LogTransaction($"PayPal payment processed for {amount} USD");
        }
    }
}