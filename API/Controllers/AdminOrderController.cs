using Application.Queries.Orders.GetAdminOrders;
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
    }
}
