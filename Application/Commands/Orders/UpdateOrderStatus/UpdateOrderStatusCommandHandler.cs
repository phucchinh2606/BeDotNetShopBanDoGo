using Application.Commons.DTOs;
using Application.Commons.Exceptions;
using Application.Commons.Models;
using AutoMapper;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Orders.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, ApiResponse<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateOrderStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<OrderDto>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            // 1. Tìm đơn hàng cần cập nhật (Bao gồm OrderDetails để trả về DTO đầy đủ)
            var order = await _unitOfWork.Orders.GetOrderByIdWithDetailsAsync(request.OrderId);
            if (order == null)
            {
                throw new NotFoundException("Đơn hàng", request.OrderId);
            }

            // 2. Logic hoàn tồn kho nếu đơn hàng bị hủy (Nếu đơn chưa bị hủy trước đó)
            if (request.OrderStatus == OrderStatus.Cancelled && order.OrderStatus != OrderStatus.Cancelled)
            {
                foreach (var detail in order.OrderDetails)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(detail.ProductId);
                    if (product != null)
                    {
                        product.StockQuantity += detail.Quantity;
                        _unitOfWork.Products.Update(product);
                    }
                }
            }

            // 3. Cập nhật trạng thái
            order.OrderStatus = request.OrderStatus;
            order.PaymentStatus = request.PaymentStatus;

            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();

            // 4. Mapping và trả về kết quả
            var orderDto = _mapper.Map<OrderDto>(order);
            return ApiResponse<OrderDto>.SuccessResult(orderDto, "Cập nhật trạng thái đơn hàng thành công.");
        }
    }
}
