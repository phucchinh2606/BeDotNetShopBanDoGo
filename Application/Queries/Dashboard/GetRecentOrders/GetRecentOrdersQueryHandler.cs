using Application.Commons.DTOs;
using Application.Commons.Models;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.Dashboard.GetRecentOrders
{
    public class GetRecentOrdersQueryHandler : IRequestHandler<GetRecentOrdersQuery, ApiResponse<IEnumerable<RecentOrderDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRecentOrdersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<IEnumerable<RecentOrderDto>>> Handle(GetRecentOrdersQuery request, CancellationToken cancellationToken)
        {
            // 1. Lấy tất cả danh sách đơn hàng và sắp xếp theo ngày đặt giảm dần
            var orders = await _unitOfWork.Orders.GetAllAsync();
            var recentOrders = orders
                .OrderByDescending(o => o.OrderDate)
                .Take(request.Limit)
                .ToList();

            var recentOrderDtos = new List<RecentOrderDto>();

            // 2. Map thông tin đơn hàng cùng thông tin khách hàng và tổng số lượng sản phẩm
            foreach (var order in recentOrders)
            {
                var user = await _unitOfWork.Users.GetByIdAsync(order.UserId);

                // Lấy chi tiết đơn hàng để đếm tổng số món hàng
                var orderDetails = await _unitOfWork.OrderDetails.GetAllAsync();
                var totalItems = orderDetails
                    .Where(od => od.OrderId == order.OrderId)
                    .Sum(od => od.Quantity);

                recentOrderDtos.Add(new RecentOrderDto
                {
                    OrderId = order.OrderId,
                    UserId = order.UserId,
                    CustomerName = user?.FullName ?? "N/A",
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    PaymentMethod = order.PaymentMethod,
                    PaymentStatus = order.PaymentStatus,
                    OrderStatus = order.OrderStatus,
                    TotalItems = totalItems
                });
            }

            return ApiResponse<IEnumerable<RecentOrderDto>>.SuccessResult(
                recentOrderDtos,
                $"Lấy danh sách {recentOrderDtos.Count} đơn hàng mới nhất thành công."
            );
        }
    }
}
