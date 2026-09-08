using Application.Commons.DTOs;
using Application.Commons.Models;
using Domain.Enums;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Commands.Orders.CreateOrder
{
    public class CreateOrderCommand : IRequest<ApiResponse<OrderDto>>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }

        public string ShippingAddress { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Note { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.COD;

        // Danh sách các sản phẩm/mục giỏ hàng được người dùng tích chọn
        public List<Guid> SelectedCartItemIds { get; set; } = new List<Guid>();
    }
}