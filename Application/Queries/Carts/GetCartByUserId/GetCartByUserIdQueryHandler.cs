using Application.Commons.DTOs;
using Application.Commons.Models;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.Carts.GetCartByUserId
{
    public class GetCartByUserIdQueryHandler : IRequestHandler<GetCartByUserIdQuery, ApiResponse<CartDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCartByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<CartDto>> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
        {
            // 1. Tìm giỏ hàng theo UserId (đã bao gồm CartItems và Product)
            var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(request.UserId);

            // 2. Nếu chưa có giỏ hàng, trả về giỏ hàng rỗng
            if (cart == null)
            {
                var emptyCartDto = new CartDto
                {
                    CartId = Guid.Empty,
                    UserId = request.UserId,
                    Items = new List<CartItemDto>()
                };
                return ApiResponse<CartDto>.SuccessResult(emptyCartDto, "Giỏ hàng hiện đang trống.");
            }

            // 3. Mapping sang CartDto
            var cartDto = _mapper.Map<CartDto>(cart);

            return ApiResponse<CartDto>.SuccessResult(cartDto, "Lấy thông tin giỏ hàng thành công.");
        }
    }
}
