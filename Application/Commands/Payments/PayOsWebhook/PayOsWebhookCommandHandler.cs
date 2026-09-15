using Application.Commons.Models;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using PayOS;
using PayOS.Models.Webhooks;

namespace Application.Commands.Payments.PayOsWebhook
{
    public class PayOsWebhookCommandHandler : IRequestHandler<PayOsWebhookCommand, ApiResponse<string>>
    {
        private readonly PayOSClient _payOSClient;
        private readonly IUnitOfWork _unitOfWork; // Inject IUnitOfWork vào Handler

        public PayOsWebhookCommandHandler(PayOSClient payOSClient, IUnitOfWork unitOfWork)
        {
            _payOSClient = payOSClient;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<string>> Handle(PayOsWebhookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Verify Webhook theo SDK payOS
                WebhookData verifiedData = await _payOSClient.Webhooks.VerifyAsync(request.WebhookBody);

                if (verifiedData != null)
                {
                    long orderCode = verifiedData.OrderCode;

                    // Tìm đơn hàng bằng OrderCode
                    var order = await _unitOfWork.Orders.GetByOrderCodeAsync(orderCode);
                    if (order != null)
                    {
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
                    }
                }

                return ApiResponse<string>.SuccessResult("Xử lý Webhook thành công.");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailureResult($"Chữ ký Webhook không hợp lệ: {ex.Message}");
            }
        }
    }
}
