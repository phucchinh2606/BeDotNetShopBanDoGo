namespace Application.Commons.DTOs
{
    public class DashboardSummaryDto
    {
        public decimal TotalRevenue { get; set; }           // Tổng doanh thu (các đơn hàng Delivered)
        public int TotalOrders { get; set; }               // Tổng số lượng đơn hàng
        public int PendingOrders { get; set; }             // Đơn hàng đang chờ xử lý
        public int CompletedOrders { get; set; }           // Đơn hàng giao thành công
        public int CancelledOrders { get; set; }           // Đơn hàng đã hủy
        public int TotalProducts { get; set; }             // Tổng số sản phẩm
        public int LowStockProducts { get; set; }          // Sản phẩm sắp hết hàng (Stock <= 5)
        public int TotalCustomers { get; set; }            // Tổng số khách hàng (Role = Customer)
    }
}
