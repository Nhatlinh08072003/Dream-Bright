namespace Dream_Bright.Services.Adapters
{
    public interface IPaymentGateway
    {
        void ProcessPayment(decimal amount);
    }
}