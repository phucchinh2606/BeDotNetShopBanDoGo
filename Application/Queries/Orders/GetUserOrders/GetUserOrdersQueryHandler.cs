using Application.Commons.DTOs;
using Application.Commons.Models;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.Orders.GetUserOrders
{
    public class GetUserOrdersQueryHandler : IRequestHandler<GetUserOrdersQuery, ApiResponse<List<OrderDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserOrdersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<OrderDto>>> Handle(GetUserOrdersQuery request, CancellationToken cancellationToken)
        {
            // 1. Lấy danh sách đơn hàng theo UserId (Cần Include OrderDetails và Product ở Repository)
            var orders = await _unitOfWork.Orders.GetOrdersByUserIdAsync(request.UserId);

            if (orders == null || !orders.Any())
            {
                return ApiResponse<List<OrderDto>>.SuccessResult(new List<OrderDto>(), "Bạn chưa có đơn hàng nào.");
            }

            // 2. Mapping danh sách Order sang OrderDto
            var orderDtos = _mapper.Map<List<OrderDto>>(orders);

            return ApiResponse<List<OrderDto>>.SuccessResult(orderDtos, "Lấy danh sách đơn hàng thành công.");
        }
    }
}
