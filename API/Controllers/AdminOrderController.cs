using Application.Queries.Orders.GetAdminOrders;
using Application.Queries.Orders.GetOrderById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/admin/orders")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Yêu cầu tài khoản phải có Role Admin
    public class AdminOrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminOrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] GetAdminOrdersQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOrderByIdForAdmin(Guid id)
        {
            var query = new GetOrderByIdQuery(id, userId: null); // UserId = null cho phép Admin xem mọi đơn hàng
            var result = await _mediator.Send(query);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
