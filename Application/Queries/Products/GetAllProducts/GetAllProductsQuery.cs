using Application.Commons.DTOs;
using Application.Commons.Models;
using Domain.Enums;
using MediatR;

namespace Application.Queries.Products.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<ApiResponse<PagedResult<ProductDto>>>
    {
        public string? SearchTerm { get; set; }
        public Guid? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public ProductStatus? Status { get; set; }
        public string? SortBy { get; set; } = "createdAt";
        public bool IsDescending { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
