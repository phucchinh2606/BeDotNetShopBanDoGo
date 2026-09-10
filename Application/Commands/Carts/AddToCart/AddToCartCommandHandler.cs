using Application.Commons.DTOs;
using Application.Commons.Exceptions;
using Application.Commons.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Carts.AddToCart
{
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, ApiResponse<CartDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddToCartCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<CartDto>> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            if (request.Quantity <= 0)
            {
                throw new BadRequestException("Số lượng sản phẩm phải lớn hơn 0.");
            }

            // 1. Kiểm tra sản phẩm có tồn tại không
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new NotFoundException("Sản phẩm", request.ProductId);
            }

            // 2. Kiểm tra số lượng tồn kho
            if (product.StockQuantity < request.Quantity)
            {
                throw new BadRequestException($"Sản phẩm chỉ còn lại {product.StockQuantity} trong kho.");
            }

            // 3. Lấy hoặc tạo Giỏ hàng cho User
            var cart = await _unitOfWork.Carts.GetOrCreateCartAsync(request.UserId);

            // 4. Kiểm tra sản phẩm đã có trong giỏ chưa
            var existingCartItem = await _unitOfWork.CartItems.GetCartItemAsync(cart.CartId, request.ProductId);

            if (existingCartItem != null)
            {
                int newQuantity = existingCartItem.Quantity + request.Quantity;
                if (product.StockQuantity < newQuantity)
                {
                    throw new BadRequestException($"Không thể thêm. Tổng số lượng trong giỏ ({newQuantity}) vượt quá số lượng tồn kho ({product.StockQuantity}).");
                }
                existingCartItem.Quantity = newQuantity;
                _unitOfWork.CartItems.Update(existingCartItem);
            }
            else
            {
                var newCartItem = new CartItem
                {
                    CartItemId = Guid.NewGuid(),
                    CartId = cart.CartId,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    CreatedAt = DateTime.UtcNow
                };
                await _unitOfWork.CartItems.AddAsync(newCartItem);
            }

            cart.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync();

            // 5. Lấy giỏ hàng cập nhật mới nhất cùng dữ liệu liên quan
            var updatedCart = await _unitOfWork.Carts.GetCartByUserIdAsync(request.UserId);
            var cartDto = _mapper.Map<CartDto>(updatedCart);

            return ApiResponse<CartDto>.SuccessResult(cartDto, "Thêm sản phẩm vào giỏ hàng thành công.");
        }
    }
}
