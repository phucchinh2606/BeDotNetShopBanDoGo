using Application.Commons.DTOs;
using Application.Commons.Models;
using Application.Queries.Dashboard.GetDashboardSummary;
using Application.Queries.Dashboard.GetRecentOrders;
using Application.Queries.Dashboard.GetRevenueChart;
using Application.Queries.Dashboard.GetTopProducts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/admin/dashboard")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminDashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("summary")]
        public async Task<ActionResult<ApiResponse<DashboardSummaryDto>>> GetSummary()
        {
            var result = await _mediator.Send(new GetDashboardSummaryQuery());
            return Ok(result);
        }

        [HttpGet("revenue-chart")]
        public async Task<ActionResult<ApiResponse<RevenueChartDto>>> GetRevenueChart(
            [FromQuery] string period = "month",
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var query = new GetRevenueChartQuery
            {
                Period = period,
                FromDate = fromDate,
                ToDate = toDate
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("top-products")]
        public async Task<ActionResult<ApiResponse<IEnumerable<TopProductDto>>>> GetTopProducts([FromQuery] int limit = 5)
        {
            var query = new GetTopProductsQuery { Limit = limit };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("recent-orders")]
        public async Task<ActionResult<ApiResponse<IEnumerable<RecentOrderDto>>>> GetRecentOrders([FromQuery] int limit = 5)
        {
            var query = new GetRecentOrdersQuery { Limit = limit };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
