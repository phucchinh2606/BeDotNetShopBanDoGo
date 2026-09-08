using Application.Commands.Carts.AddToCart;
using Application.Commands.Carts.RemoveFromCart;
using Application.Commands.Carts.UpdateCartItemQuantity;
using Application.Commons.Models;
using Application.Queries.Carts.GetCartByUserId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Yêu cầu khách hàng phải đăng nhập
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartCommand command)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(ApiResponse<object>.FailureResult("Bạn chưa đăng nhập hoặc Token không hợp lệ."));
            }

            command.UserId = userId;
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("update-quantity")]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateCartItemQuantityCommand command)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(ApiResponse<object>.FailureResult("Bạn chưa đăng nhập hoặc Token không hợp lệ."));
            }

            command.UserId = userId;
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("remove/{productId:guid}")]
        public async Task<IActionResult> RemoveFromCart([FromRoute] Guid productId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(ApiResponse<object>.FailureResult("Bạn chưa đăng nhập hoặc Token không hợp lệ."));
            }

            var command = new RemoveFromCartCommand
            {
                UserId = userId,
                ProductId = productId
            };

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(ApiResponse<object>.FailureResult("Bạn chưa đăng nhập hoặc Token không hợp lệ."));
            }

            var query = new GetCartByUserIdQuery(userId);
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
