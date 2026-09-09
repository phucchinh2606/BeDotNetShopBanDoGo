using Domain.Enums;

namespace Application.Commons.DTOs
{
    public class RecentOrderDto
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public PaymentStatus PaymentStatus { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int TotalItems { get; set; } // Tổng số lượng sản phẩm trong đơn
    }
}
