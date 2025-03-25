// Thêm adapter factory
using Dream_Bright.Services.Adapters;
using Dream_Bright.Services.Logging;
using Dream_Bright.Services.PaymentGateways;
public static class PaymentAdapterFactory
{
    public static IPaymentGateway CreateAdapter(string gatewayType, ITransactionLogger logger)
    {
        return gatewayType switch
        {
            "PayPal" => new PayPalAdapter(new PayPalService(logger)),
            "VNPay" => new VNPayAdapter(new VNPayService(logger)),
            _ => throw new ArgumentException("Invalid payment gateway")
        };
    }
}