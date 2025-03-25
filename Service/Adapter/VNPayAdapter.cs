using Dream_Bright.Services.PaymentGateways;
using Dream_Bright.Services.Logging;
namespace Dream_Bright.Services.Adapters
{
    public class VNPayAdapter : IPaymentGateway
    {
        private readonly VNPayService _vnpayService;

        public VNPayAdapter(VNPayService vnpayService)
        {
            _vnpayService = vnpayService;
        }

        public void ProcessPayment(decimal amount)
        {
            // Chuyển đổi từ interface chung sang API VNPay cụ thể
            _vnpayService.Pay(amount);
        }
    }
}