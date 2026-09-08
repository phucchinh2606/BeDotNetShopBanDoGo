using Application.Commons.DTOs;
using Application.Commons.Models;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.Orders.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, ApiResponse<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Orders.GetOrderByIdWithDetailsAsync(request.OrderId);

            if (order == null)
            {
                return ApiResponse<OrderDto>.FailureResult("Không tìm thấy đơn hàng.");
            }

            // Nếu request đến từ Client (có UserId), kiểm tra xem đơn hàng này có thuộc về họ không
            if (request.UserId.HasValue && order.UserId != request.UserId.Value)
            {
                return ApiResponse<OrderDto>.FailureResult("Bạn không có quyền truy cập đơn hàng này.");
            }

            var orderDto = _mapper.Map<OrderDto>(order);

            return ApiResponse<OrderDto>.SuccessResult(orderDto, "Lấy thông tin chi tiết đơn hàng thành công.");
        }
    }
}
