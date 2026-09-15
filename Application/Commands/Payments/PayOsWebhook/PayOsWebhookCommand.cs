using Application.Commons.Models;
using MediatR;
using PayOS.Models.Webhooks;

namespace Application.Commands.Payments.PayOsWebhook
{
    public class PayOsWebhookCommand : IRequest<ApiResponse<string>>
    {
        // Đổi từ WebhookData sang Webhook
        public Webhook WebhookBody { get; set; }

        public PayOsWebhookCommand(Webhook webhookBody)
        {
            WebhookBody = webhookBody;
        }
    }
}
