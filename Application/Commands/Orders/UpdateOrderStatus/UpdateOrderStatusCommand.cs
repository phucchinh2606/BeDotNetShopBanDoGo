using Application.Commons.DTOs;
using Application.Commons.Models;
using Domain.Enums;
using MediatR;

namespace Application.Commands.Orders.UpdateOrderStatus
{
    public class UpdateOrderStatusCommand : IRequest<ApiResponse<OrderDto>>
    {
        public Guid OrderId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}
