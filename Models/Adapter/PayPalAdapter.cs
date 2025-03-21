public class PayPalAdapter : IPaymentGateway
{
    private readonly PayPalService _paypalService;

    public PayPalAdapter(PayPalService paypalService)
    {
        _paypalService = paypalService;
    }

    public void ProcessPayment(decimal amount)
    {
        _paypalService.MakePayment(amount);
    }
}