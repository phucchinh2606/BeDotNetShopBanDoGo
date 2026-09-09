using Application.Commons.DTOs;
using Application.Commons.Models;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.Dashboard.GetDashboardSummary
{
    public class GetDashboardSummaryQueryHandler : IRequestHandler<GetDashboardSummaryQuery, ApiResponse<DashboardSummaryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDashboardSummaryQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<DashboardSummaryDto>> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
        {
            // 1. Lấy tất cả đơn hàng
            var orders = await _unitOfWork.Orders.GetAllAsync();
            var orderList = orders.ToList();

            // Tính toán thống kê Đơn hàng & Doanh thu
            var totalOrders = orderList.Count;
            var pendingOrders = orderList.Count(o => o.OrderStatus == OrderStatus.Pending);
            var completedOrders = orderList.Count(o => o.OrderStatus == OrderStatus.Delivered);
            var cancelledOrders = orderList.Count(o => o.OrderStatus == OrderStatus.Cancelled);

            // Chỉ tính doanh thu từ những đơn đã hoàn thành (Delivered)
            var totalRevenue = orderList
                .Where(o => o.OrderStatus == OrderStatus.Delivered)
                .Sum(o => o.TotalAmount);

            // 2. Lấy thông tin Sản phẩm
            var products = await _unitOfWork.Products.GetAllAsync();
            var productList = products.ToList();
            var totalProducts = productList.Count;
            var lowStockProducts = productList.Count(p => p.StockQuantity <= 5); // Cảnh báo khi tồn kho <= 5

            // 3. Lấy thông tin Khách hàng (Chỉ đếm tài khoản Customer)
            var users = await _unitOfWork.Users.GetAllAsync();
            var totalCustomers = users.Count(u => u.Role == UserRole.Customer);

            // 4. Bọc vào DTO
            var summary = new DashboardSummaryDto
            {
                TotalRevenue = totalRevenue,
                TotalOrders = totalOrders,
                PendingOrders = pendingOrders,
                CompletedOrders = completedOrders,
                CancelledOrders = cancelledOrders,
                TotalProducts = totalProducts,
                LowStockProducts = lowStockProducts,
                TotalCustomers = totalCustomers
            };

            return ApiResponse<DashboardSummaryDto>.SuccessResult(summary, "Lấy thông số tổng quan thành công.");
        }
    }
}
