using Application.Commands.Payments.PayOsWebhook;
using Application.Commands.Payments.ConfirmPayOsPayment;
using Application.Commons.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayOS.Models.Webhooks;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("payos-webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> PayOSWebhook([FromBody] Webhook webhookBody)
        {
            var result = await _mediator.Send(new PayOsWebhookCommand(webhookBody));
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("payos/{orderId:guid}/confirm")]
        [Authorize]
        public async Task<IActionResult> ConfirmPayOSPayment(Guid orderId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(ApiResponse<object>.FailureResult("Bạn chưa đăng nhập hoặc Token không hợp lệ."));
            }

            var result = await _mediator.Send(new ConfirmPayOsPaymentCommand(orderId, userId));
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
