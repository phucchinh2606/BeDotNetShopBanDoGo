using Application.Commons.DTOs;
using Application.Commons.Models;
using MediatR;

namespace Application.Queries.Dashboard.GetTopProducts
{
    public class GetTopProductsQuery : IRequest<ApiResponse<IEnumerable<TopProductDto>>>
    {
        public int Limit { get; set; } = 5; // Mặc định lấy Top 5 sản phẩm
    }
}
