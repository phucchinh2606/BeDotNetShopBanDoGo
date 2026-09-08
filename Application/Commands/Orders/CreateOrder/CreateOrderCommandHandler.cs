using Application.Commons.DTOs;
using Application.Commons.Interfaces;
using Application.Commons.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Orders.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResponse<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentFactory _paymentFactory;

        public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPaymentFactory paymentFactory)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentFactory = paymentFactory;
        }

        public async Task<ApiResponse<OrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.ShippingAddress))
            {
                return ApiResponse<OrderDto>.FailureResult("Địa chỉ giao hàng không được để trống.");
            }

            // 1. Kiểm tra danh sách sản phẩm được chọn
            if (request.SelectedCartItemIds == null || !request.SelectedCartItemIds.Any())
            {
                return ApiResponse<OrderDto>.FailureResult("Vui lòng chọn ít nhất một sản phẩm để thanh toán.");
            }

            // 2. Lấy giỏ hàng của User
            var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(request.UserId);
            if (cart == null || !cart.CartItems.Any())
            {
                return ApiResponse<OrderDto>.FailureResult("Giỏ hàng của bạn đang trống.");
            }

            // 3. Lọc danh sách CartItems theo danh sách SelectedCartItemIds người dùng chọn
            var selectedCartItems = cart.CartItems
                .Where(item => request.SelectedCartItemIds.Contains(item.CartItemId))
                .ToList();

            if (!selectedCartItems.Any())
            {
                return ApiResponse<OrderDto>.FailureResult("Các sản phẩm được chọn không tồn tại trong giỏ hàng.");
            }

            // 4. Kiểm tra tồn kho cho các sản phẩm ĐÃ CHỌN
            foreach (var item in selectedCartItems)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                if (product == null || product.StockQuantity < item.Quantity)
                {
                    return ApiResponse<OrderDto>.FailureResult($"Sản phẩm '{item.Product?.ProductName}' không đủ số lượng tồn kho.");
                }
            }

            // 5. Khai báo Order và tính tổng tiền dựa trên selectedCartItems
            decimal totalAmount = selectedCartItems.Sum(item => item.Quantity * item.Product.Price);
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                UserId = request.UserId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                ShippingAddress = request.ShippingAddress,
                PaymentMethod = request.PaymentMethod.ToString()
            };

            // 6. Chuyển đổi selectedCartItems sang OrderDetail & Trừ tồn kho
            foreach (var item in selectedCartItems)
            {
                order.OrderDetails.Add(new OrderDetail
                {
                    OrderDetailId = Guid.NewGuid(),
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price
                });

                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.StockQuantity -= item.Quantity;
                    _unitOfWork.Products.Update(product);
                }
            }

            // 7. Gọi IPaymentService qua PaymentFactory (Giữ nguyên Factory Pattern cho thanh toán)
            var paymentService = _paymentFactory.GetPaymentService(request.PaymentMethod);
            var paymentResult = await paymentService.ProcessPaymentAsync(order);

            if (!paymentResult.IsSuccess)
            {
                return ApiResponse<OrderDto>.FailureResult($"Thanh toán thất bại: {paymentResult.Message}");
            }

            // 8. Lưu đơn hàng
            await _unitOfWork.Orders.AddAsync(order);

            // 9. CHỈ XÓA CÁC CARTITEM ĐÃ ĐƯỢC CHỌN (Không xóa sạch cả giỏ hàng)
            foreach (var item in selectedCartItems)
            {
                _unitOfWork.CartItems.Delete(item);
            }

            await _unitOfWork.SaveChangesAsync();

            var orderDto = _mapper.Map<OrderDto>(order);
            orderDto.PaymentUrl = paymentResult.PaymentUrl;

            return ApiResponse<OrderDto>.SuccessResult(orderDto, paymentResult.Message);
        }
    }
}
