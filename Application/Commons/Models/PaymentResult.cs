namespace Application.Commons.Models
{
    public class PaymentResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? PaymentUrl { get; set; } // Dùng cho VNPay, Momo...
        public string? TransactionId { get; set; }
    }
}
