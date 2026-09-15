using Application.Commons.Models;
using MediatR;

namespace Application.Commands.Payments.ConfirmPayOsPayment
{
    public class ConfirmPayOsPaymentCommand : IRequest<ApiResponse<bool>>
    {
        public Guid OrderId { get; }
        public Guid UserId { get; }

        public ConfirmPayOsPaymentCommand(Guid orderId, Guid userId)
        {
            OrderId = orderId;
            UserId = userId;
        }
    }
}
