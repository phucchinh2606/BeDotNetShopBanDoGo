using Application.Commons.DTOs;
using Application.Commons.Models;
using MediatR;

namespace Application.Queries.Products.GetProductById
{
    public class GetProductByIdQuery : IRequest<ApiResponse<ProductDto>>
    {
        public Guid ProductId { get; set; }

        public GetProductByIdQuery(Guid productId)
        {
            ProductId = productId;
        }
    }
}
