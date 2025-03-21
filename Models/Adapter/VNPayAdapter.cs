public class VNPayAdapter : IPaymentGateway
{
    private readonly VNPayService _vnpayService;

    public VNPayAdapter(VNPayService vnpayService)
    {
        _vnpayService = vnpayService;
    }

    public void ProcessPayment(decimal amount)
    {
        _vnpayService.Pay(amount);
    }
}