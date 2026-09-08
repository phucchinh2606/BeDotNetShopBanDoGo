using Application.Commons.DTOs;
using Application.Commons.Models;
using Domain.Enums;
using MediatR;

namespace Application.Queries.Orders.GetAdminOrders
{
    public class GetAdminOrdersQuery : IRequest<ApiResponse<PagedResult<OrderDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public OrderStatus? OrderStatus { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public string? SearchTerm { get; set; } // Tìm theo Tên khách hàng, SĐT hoặc Mã đơn
    }
}
