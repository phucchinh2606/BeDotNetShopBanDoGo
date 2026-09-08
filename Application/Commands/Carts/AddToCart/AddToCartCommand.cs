using Application.Commons.DTOs;
using Application.Commons.Models;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Commands.Carts.AddToCart
{
    public class AddToCartCommand : IRequest<ApiResponse<CartDto>>
    {
        [JsonIgnore] // Lấy UserId từ Token JWT của người dùng đã đăng nhập
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
