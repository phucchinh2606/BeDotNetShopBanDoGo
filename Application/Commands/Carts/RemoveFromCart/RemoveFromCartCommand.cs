using Application.Commons.DTOs;
using Application.Commons.Models;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Commands.Carts.RemoveFromCart
{
    public class RemoveFromCartCommand : IRequest<ApiResponse<CartDto>>
    {
        [JsonIgnore] // Lấy UserId từ Token JWT
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }

        public RemoveFromCartCommand() { }

        public RemoveFromCartCommand(Guid productId)
        {
            ProductId = productId;
        }
    }
}
