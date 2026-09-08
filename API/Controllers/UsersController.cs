using Application.Commands.Users.CreateUser;
using Application.Commands.Users.DeleteUser;
using Application.Commands.Users.UpdateUser;
using Application.Commons.DTOs;
using Application.Commons.Models;
using Application.Queries.Users.GetAllUsers;
using Application.Queries.Users.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Bắt buộc phải đăng nhập và có quyền Admin
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            try
            {
                var userId = await _mediator.Send(command);
                return Ok(ApiResponse<Guid>.SuccessResult(userId, "Tạo người dùng mới thành công."));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _mediator.Send(new GetAllUsersQuery());
                return Ok(ApiResponse<List<UserDto>>.SuccessResult(result, "Lấy danh sách người dùng thành công."));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(System.Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetUserByIdQuery(id));
                return Ok(ApiResponse<UserDto>.SuccessResult(result, "Lấy thông tin người dùng thành công."));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(System.Guid id, [FromBody] UpdateUserCommand command)
        {
            try
            {
                command.UserId = id; // Đảm bảo UserId trong body khớp với id trên URL
                var result = await _mediator.Send(command);
                return Ok(ApiResponse<bool>.SuccessResult(result, "Cập nhật thông tin người dùng thành công."));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(System.Guid id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteUserCommand(id));
                return Ok(ApiResponse<bool>.SuccessResult(result, "Xóa người dùng thành công."));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
            }
        }
    }
}
