using Application.Commands.Products.CreateProduct;
using Application.Commands.Products.DeleteProduct;
using Application.Commands.Products.UpdateProduct;
using Application.Commons.Models;
using Application.Queries.Products.GetAllProducts;
using Application.Queries.Products.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")] // Định dạng bắt buộc khi gửi kèm File
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductCommand command)
        {
            try
            {
                var productId = await _mediator.Send(command);
                return Ok(ApiResponse<Guid>.SuccessResult(productId, "Tạo sản phẩm thành công."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllProductsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromForm] UpdateProductCommand command)
        {
            command.ProductId = id;
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id));

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
