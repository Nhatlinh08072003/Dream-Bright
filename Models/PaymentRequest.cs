// Dream-Bright/Models/PaymentRequest.cs
namespace Dream_Bright.Models
{
    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string Gateway { get; set; } = "PayPal";
        public string Description { get; set; } = string.Empty;
    }
}