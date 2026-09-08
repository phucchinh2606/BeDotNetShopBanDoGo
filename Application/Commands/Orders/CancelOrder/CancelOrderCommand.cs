using Application.Commons.DTOs;
using Application.Commons.Models;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Commands.Orders.CancelOrder
{
    public class CancelOrderCommand : IRequest<ApiResponse<OrderDto>>
    {
        public Guid OrderId { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }

        public string? CancelReason { get; set; }
    }
}
