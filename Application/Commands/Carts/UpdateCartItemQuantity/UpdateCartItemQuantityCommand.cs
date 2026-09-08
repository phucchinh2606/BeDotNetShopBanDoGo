using Application.Commons.DTOs;
using Application.Commons.Models;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Commands.Carts.UpdateCartItemQuantity
{
    public class UpdateCartItemQuantityCommand : IRequest<ApiResponse<CartDto>>
    {
        [JsonIgnore] // Lấy UserId từ Token JWT
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
