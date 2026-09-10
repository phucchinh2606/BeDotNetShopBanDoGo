using Application.Commons.DTOs;
using Application.Commons.Exceptions;
using Application.Commons.Models;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Carts.RemoveFromCart
{
    public class RemoveFromCartCommandHandler : IRequestHandler<RemoveFromCartCommand, ApiResponse<CartDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RemoveFromCartCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<CartDto>> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
        {
            // 1. Lấy thông tin giỏ hàng của User
            var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(request.UserId);
            if (cart == null)
            {
                throw new NotFoundException("Không tìm thấy giỏ hàng của người dùng.");
            }

            // 2. Kiểm tra sản phẩm có nằm trong giỏ hàng không
            var cartItem = await _unitOfWork.CartItems.GetCartItemAsync(cart.CartId, request.ProductId);
            if (cartItem == null)
            {
                throw new NotFoundException("Sản phẩm không có trong giỏ hàng.");
            }

            // 3. Thực hiện xóa item khỏi giỏ hàng
            _unitOfWork.CartItems.Delete(cartItem);

            cart.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync();

            // 4. Lấy lại giỏ hàng cập nhật và trả về
            var updatedCart = await _unitOfWork.Carts.GetCartByUserIdAsync(request.UserId);
            var cartDto = _mapper.Map<CartDto>(updatedCart);

            return ApiResponse<CartDto>.SuccessResult(cartDto, "Xóa sản phẩm khỏi giỏ hàng thành công.");
        }
    }
}
