using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order> {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId);

        // Bổ sung hàm lấy tất cả đơn hàng có phân trang & lọc cho Admin
        Task<(IEnumerable<Order> Orders, int TotalCount)> GetAdminOrdersAsync(
            int pageNumber,
            int pageSize,
            OrderStatus? orderStatus,
            PaymentStatus? paymentStatus,
            string? searchTerm);

        // Bổ sung hàm lấy chi tiết 1 đơn hàng bao gồm OrderDetails & Product
        Task<Order?> GetOrderByIdWithDetailsAsync(Guid orderId);

        // Dùng cho Command (Hủy đơn, Cập nhật trạng thái) - Có Tracking
        Task<Order?> GetOrderByIdForUpdateAsync(Guid orderId);
    }
}
