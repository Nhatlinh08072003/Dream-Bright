using Dream_Bright.Services.PaymentGateways;
using Dream_Bright.Services.Logging;
namespace Dream_Bright.Services.Adapters
{
    public class PayPalAdapter : IPaymentGateway
    {
        private readonly PayPalService _paypalService;

        public PayPalAdapter(PayPalService paypalService)
        {
            _paypalService = paypalService;
        }

        public void ProcessPayment(decimal amount)
        {
            // Chuyển đổi từ interface chung sang API PayPal cụ thể
            _paypalService.MakePayment(amount);
        }
    }
}