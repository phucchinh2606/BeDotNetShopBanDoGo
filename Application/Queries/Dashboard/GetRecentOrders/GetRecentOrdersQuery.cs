using Application.Commons.DTOs;
using Application.Commons.Models;
using MediatR;

namespace Application.Queries.Dashboard.GetRecentOrders
{
    public class GetRecentOrdersQuery : IRequest<ApiResponse<IEnumerable<RecentOrderDto>>>
    {
        public int Limit { get; set; } = 5; // Mặc định lấy 5 đơn hàng mới nhất
    }
}
