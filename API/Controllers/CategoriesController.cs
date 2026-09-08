using Application.Commands.Categories.CreateCategory;
using Application.Commands.Categories.DeleteCategory;
using Application.Commands.Categories.UpdateCategory;
using Application.Commons.DTOs;
using Application.Commons.Models;
using Application.Queries.Categories.GetAllCategories;
using Application.Queries.Categories.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Bảo vệ endpoint chỉ cho phép Admin
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
        {
            try
            {
                var categoryId = await _mediator.Send(command);
                return Ok(ApiResponse<Guid>.SuccessResult(categoryId, "Tạo danh mục thành công."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
            }
        }

        [HttpGet]
        [AllowAnonymous] // Cho phép tất cả người dùng lấy cây danh mục
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var result = await _mediator.Send(new GetAllCategoriesQuery());
                return Ok(ApiResponse<List<CategoryDto>>.SuccessResult(result, "Lấy danh sách danh mục thành công."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryCommand command)
        {
            try
            {
                command.CategoryId = id; // Đảm bảo CategoryId trùng với id trên URL
                var result = await _mediator.Send(command);
                return Ok(ApiResponse<bool>.SuccessResult(result, "Cập nhật danh mục thành công."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteCategoryCommand(id));
                return Ok(ApiResponse<bool>.SuccessResult(result, "Xóa danh mục thành công."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous] // Cho phép tất cả xem chi tiết danh mục
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetCategoryByIdQuery(id));
                return Ok(ApiResponse<CategoryDto>.SuccessResult(result, "Lấy thông tin danh mục thành công."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
            }
        }
    }
}
