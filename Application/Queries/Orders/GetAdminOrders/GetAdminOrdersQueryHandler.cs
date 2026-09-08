using Application.Commons.DTOs;
using Application.Commons.Models;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.Orders.GetAdminOrders
{
    public class GetAdminOrdersQueryHandler : IRequestHandler<GetAdminOrdersQuery, ApiResponse<PagedResult<OrderDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAdminOrdersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PagedResult<OrderDto>>> Handle(GetAdminOrdersQuery request, CancellationToken cancellationToken)
        {
            var (orders, totalCount) = await _unitOfWork.Orders.GetAdminOrdersAsync(
                request.PageNumber,
                request.PageSize,
                request.OrderStatus,
                request.PaymentStatus,
                request.SearchTerm
            );

            var orderDtos = _mapper.Map<List<OrderDto>>(orders);
            var pagedResult = new PagedResult<OrderDto>(orderDtos, totalCount, request.PageNumber, request.PageSize);

            return ApiResponse<PagedResult<OrderDto>>.SuccessResult(pagedResult, "Lấy danh sách đơn hàng cho Admin thành công.");
        }
    }
}
