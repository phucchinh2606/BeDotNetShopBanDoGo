using Application.Commons.DTOs;
using Application.Commons.Models;
using AutoMapper;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Orders.CancelOrder
{
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, ApiResponse<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CancelOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<OrderDto>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            // 1. Tìm đơn hàng (Đã bao gồm OrderDetails và Product)
            var order = await _unitOfWork.Orders.GetOrderByIdForUpdateAsync(request.OrderId);
            if (order == null)
            {
                return ApiResponse<OrderDto>.FailureResult("Không tìm thấy đơn hàng.");
            }

            // 2. Kiểm tra quyền sở hữu đơn hàng của User
            if (order.UserId != request.UserId)
            {
                return ApiResponse<OrderDto>.FailureResult("Bạn không có quyền hủy đơn hàng này.");
            }

            // 3. Kiểm tra điều kiện trạng thái đơn
            if (order.OrderStatus == OrderStatus.Shipping || order.OrderStatus == OrderStatus.Delivered)
            {
                return ApiResponse<OrderDto>.FailureResult("Không thể hủy đơn hàng do đơn đã được vận chuyển hoặc giao thành công.");
            }

            if (order.OrderStatus == OrderStatus.Cancelled)
            {
                return ApiResponse<OrderDto>.FailureResult("Đơn hàng này đã được hủy trước đó.");
            }

            // 4. Hoàn lại số lượng tồn kho trực tiếp từ Navigation Property detail.Product
            foreach (var detail in order.OrderDetails)
            {
                if (detail.Product != null)
                {
                    // Tăng số lượng tồn kho trực tiếp trên entity đã được track sẵn
                    detail.Product.StockQuantity += detail.Quantity;
                }
            }

            // 5. Cập nhật trạng thái đơn hàng sang Cancelled
            order.OrderStatus = OrderStatus.Cancelled;

            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();

            // 6. Mapping và trả về kết quả
            var orderDto = _mapper.Map<OrderDto>(order);
            return ApiResponse<OrderDto>.SuccessResult(orderDto, "Hủy đơn hàng thành công.");
        }
    }
}
