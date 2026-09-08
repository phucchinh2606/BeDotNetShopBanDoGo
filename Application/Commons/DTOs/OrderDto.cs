using Domain.Enums;

namespace Application.Commons.DTOs
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public PaymentStatus PaymentStatus { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string? PaymentUrl { get; set; } // Trả về URL thanh toán nếu là VNPay/Momo/Stripe
        public List<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
    }
}
