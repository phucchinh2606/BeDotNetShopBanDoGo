using Application.Commons.DTOs;
using Application.Commons.Models;
using MediatR;

namespace Application.Queries.Orders.GetUserOrders
{
    public class GetUserOrdersQuery : IRequest<ApiResponse<List<OrderDto>>>
    {
        public Guid UserId { get; set; }

        public GetUserOrdersQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
