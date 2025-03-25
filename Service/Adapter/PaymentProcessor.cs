using Dream_Bright.Services.Adapters;
public class PaymentProcessor
{
    private readonly IPaymentGateway _paymentGateway;

    public PaymentProcessor(IPaymentGateway paymentGateway)
    {
        _paymentGateway = paymentGateway;
    }

    public void Process(decimal amount)
    {
        _paymentGateway.ProcessPayment(amount);
    }
}