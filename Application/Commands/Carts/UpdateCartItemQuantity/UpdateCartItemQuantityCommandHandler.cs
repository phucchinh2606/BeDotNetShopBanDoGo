using Application.Commons.DTOs;
using Application.Commons.Models;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Carts.UpdateCartItemQuantity
{
    public class UpdateCartItemQuantityCommandHandler : IRequestHandler<UpdateCartItemQuantityCommand, ApiResponse<CartDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCartItemQuantityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<CartDto>> Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
        {
            // 1. Ràng buộc tối thiểu số lượng phải >= 1
            if (request.Quantity < 1)
            {
                return ApiResponse<CartDto>.FailureResult("Số lượng sản phẩm trong giỏ hàng tối thiểu phải là 1.");
            }

            // 2. Lấy giỏ hàng của User
            var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(request.UserId);
            if (cart == null)
            {
                return ApiResponse<CartDto>.FailureResult("Không tìm thấy giỏ hàng của người dùng.");
            }

            // 3. Kiểm tra sản phẩm có trong giỏ hàng không
            var cartItem = await _unitOfWork.CartItems.GetCartItemAsync(cart.CartId, request.ProductId);
            if (cartItem == null)
            {
                return ApiResponse<CartDto>.FailureResult("Sản phẩm không có trong giỏ hàng.");
            }

            // 4. Kiểm tra số lượng tồn kho của sản phẩm
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return ApiResponse<CartDto>.FailureResult("Sản phẩm không tồn tại.");
            }

            if (request.Quantity > product.StockQuantity)
            {
                return ApiResponse<CartDto>.FailureResult($"Số lượng yêu cầu ({request.Quantity}) vượt quá số lượng hàng còn lại trong kho ({product.StockQuantity}).");
            }

            // 5. Cập nhật số lượng
            cartItem.Quantity = request.Quantity;
            _unitOfWork.CartItems.Update(cartItem);

            cart.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync();

            // 6. Trả về thông tin giỏ hàng mới nhất
            var updatedCart = await _unitOfWork.Carts.GetCartByUserIdAsync(request.UserId);
            var cartDto = _mapper.Map<CartDto>(updatedCart);

            return ApiResponse<CartDto>.SuccessResult(cartDto, "Cập nhật số lượng sản phẩm thành công.");
        }
    }
}
