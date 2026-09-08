using Application.Commons.Interfaces;
using Application.Commons.Models;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Services.Payments
{
    public class CodPaymentService : IPaymentService
    {
        public PaymentMethod Method => PaymentMethod.COD;

        public Task<PaymentResult> ProcessPaymentAsync(Order order)
        {
            // Với COD, không cần gọi API bên thứ 3. Khởi tạo trạng thái đơn hàng ban đầu.
            order.PaymentStatus = PaymentStatus.Unpaid;
            order.OrderStatus = OrderStatus.Pending;

            var result = new PaymentResult
            {
                IsSuccess = true,
                Message = "Xác nhận đặt hàng COD thành công.",
                TransactionId = $"COD_{order.OrderId.ToString()[..8]}"
            };

            return Task.FromResult(result);
        }
    }
}
