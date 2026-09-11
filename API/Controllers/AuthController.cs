using Application.Commands.Auth.Login;
using Application.Commands.Auth.Logout;
using Application.Commands.Auth.RefreshToken;
using Application.Commands.Auth.Register;
using Application.Commons.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            await _mediator.Send(command);
            return Ok(ApiResponse<object>.SuccessResult(null, "Đăng ký tài khoản thành công."));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<LoginResponseDto>.SuccessResult(result, "Đăng nhập thành công."));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<RefreshTokenResponseDto>.SuccessResult(result, "Làm mới Token thành công."));
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            // Lấy UserId từ Claims trong Bearer Token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Unauthorized(ApiResponse<object>.FailureResult("Token không hợp lệ."));
            }

            await _mediator.Send(new LogoutCommand(userId));
            return Ok(ApiResponse<object>.SuccessResult(null, "Đăng xuất thành công."));
        }
    }
}
