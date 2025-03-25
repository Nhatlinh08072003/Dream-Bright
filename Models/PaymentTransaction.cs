// Dream-Bright/Models/PaymentTransaction.cs
namespace Dream_Bright.Models
{
    public class PaymentTransaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public string Gateway { get; set; } = "PayPal";
        public string Status { get; set; } = "Pending";
        public DateTime Date { get; set; } = DateTime.Now;
        public string Description { get; set; } = string.Empty;

        public string FormattedAmount => Currency == "USD" ? $"{Amount:C}" : $"{Amount:N0} VND";

        public string StatusColor => Status switch
        {
            "Completed" => "bg-green-100 text-green-800",
            "Pending" => "bg-yellow-100 text-yellow-800",
            "Failed" => "bg-red-100 text-red-800",
            _ => "bg-gray-100 text-gray-800"
        };
    }
}