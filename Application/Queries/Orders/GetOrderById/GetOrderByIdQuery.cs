using Application.Commons.DTOs;
using Application.Commons.Models;
using MediatR;

namespace Application.Queries.Orders.GetOrderById
{
    public class GetOrderByIdQuery : IRequest<ApiResponse<OrderDto>>
    {
        public Guid OrderId { get; set; }
        public Guid? UserId { get; set; } // Nullable: Nếu Admin gọi thì UserId = null, nếu Client gọi thì truyền UserId để verify quyền sở hữu

        public GetOrderByIdQuery(Guid orderId, Guid? userId = null)
        {
            OrderId = orderId;
            UserId = userId;
        }
    }
}
