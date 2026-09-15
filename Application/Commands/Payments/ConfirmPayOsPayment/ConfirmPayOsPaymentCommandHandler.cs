using Application.Commons.Models;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using PayOS;

namespace Application.Commands.Payments.ConfirmPayOsPayment
{
    public class ConfirmPayOsPaymentCommandHandler
        : IRequestHandler<ConfirmPayOsPaymentCommand, ApiResponse<bool>>
    {
        private readonly PayOSClient _payOSClient;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmPayOsPaymentCommandHandler(
            PayOSClient payOSClient,
            IUnitOfWork unitOfWork)
        {
            _payOSClient = payOSClient;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<bool>> Handle(
            ConfirmPayOsPaymentCommand request,
            CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Orders.GetOrderByIdForUpdateAsync(request.OrderId);
            if (order == null || order.UserId != request.UserId)
            {
                return ApiResponse<bool>.FailureResult("Không tìm thấy đơn hàng.");
            }

            if (order.PaymentStatus == PaymentStatus.Paid)
            {
                return ApiResponse<bool>.SuccessResult(true, "Đơn hàng đã được thanh toán.");
            }

            if (order.PaymentMethod != PaymentMethod.PayOS.ToString() || order.OrderCode <= 0)
            {
                return ApiResponse<bool>.FailureResult("Đơn hàng không sử dụng thanh toán PayOS.");
            }

            try
            {
                var paymentLink = await _payOSClient.PaymentRequests.GetAsync(order.OrderCode);
                if (!string.Equals(paymentLink.Status.ToString(), "PAID", StringComparison.OrdinalIgnoreCase))
                {
                    return ApiResponse<bool>.SuccessResult(false, "Đơn hàng chưa thanh toán.");
                }

                order.PaymentStatus = PaymentStatus.Paid;
                var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(order.UserId);
                if (cart != null)
                {
                    var paidProductIds = order.OrderDetails
                        .Select(detail => detail.ProductId)
                        .ToHashSet();

                    foreach (var cartItem in cart.CartItems
                        .Where(item => paidProductIds.Contains(item.ProductId))
                        .ToList())
                    {
                        _unitOfWork.CartItems.Delete(cartItem);
                    }
                }

                _unitOfWork.Orders.Update(order);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return ApiResponse<bool>.SuccessResult(true, "Cập nhật trạng thái thanh toán thành công.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailureResult($"Không thể kiểm tra trạng thái PayOS: {ex.Message}");
            }
        }
    }
}
