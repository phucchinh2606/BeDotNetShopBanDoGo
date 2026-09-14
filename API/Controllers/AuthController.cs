using Application.Commands.Auth.Login;
using Application.Commands.Auth.Logout;
using Application.Commands.Auth.RefreshToken;
using Application.Commands.Auth.Register;
using Application.Commands.Users.ChangePassword;
using Application.Commands.Users.UpdateMyProfile;
using Application.Commons.DTOs;
using Application.Commons.Interfaces;
using Application.Commons.Models;
using Application.Queries.Users.GetUserById;
using Infrastructure.Services;
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
        private readonly ICurrentUserService _currentUserService;

        public AuthController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
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

        // ==================== MY PROFILE ENDPOINTS ====================

        // GET: /api/auth/me (Xem thông tin cá nhân)
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = _currentUserService.UserId;
            var result = await _mediator.Send(new GetUserByIdQuery(userId));
            return Ok(ApiResponse<UserDto>.SuccessResult(result, "Lấy thông tin cá nhân thành công."));
        }

        // PUT: /api/auth/me (Cập nhật thông tin cá nhân)
        [HttpPut("me")]
        [Authorize]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateMyProfileDto dto)
        {
            try
            {
                var userId = _currentUserService.UserId;
                var result = await _mediator.Send(new UpdateMyProfileCommand(userId, dto));
                return Ok(ApiResponse<UserDto>.SuccessResult(result, "Cập nhật thông tin cá nhân thành công."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
            }
        }

        // PUT: /api/auth/change-password (Đổi mật khẩu)
        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            try
            {
                var userId = _currentUserService.UserId;
                var result = await _mediator.Send(new ChangePasswordCommand(userId, dto));
                return Ok(ApiResponse<bool>.SuccessResult(result, "Đổi mật khẩu thành công."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
            }
        }
    }
}
