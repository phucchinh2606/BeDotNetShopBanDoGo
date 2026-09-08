using Application.Commands.Auth.Login;
using Application.Commands.Auth.Register;
using Application.Commons.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
            try
            {
                await _mediator.Send(command);
                return Ok(ApiResponse<object>.SuccessResult(null, "Đăng ký tài khoản thành công."));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(ApiResponse<LoginResponseDto>.SuccessResult(result, "Đăng nhập thành công."));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
            }
        }
    }
}
