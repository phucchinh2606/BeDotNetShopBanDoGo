using Application.Commons.DTOs;
using Application.Commons.Models;
using MediatR;

namespace Application.Queries.Carts.GetCartByUserId
{
    public class GetCartByUserIdQuery : IRequest<ApiResponse<CartDto>>
    {
        public Guid UserId { get; set; }

        public GetCartByUserIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
