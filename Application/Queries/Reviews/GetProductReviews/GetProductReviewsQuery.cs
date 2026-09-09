using Application.Commons.DTOs;
using Application.Commons.Models;
using MediatR;

namespace Application.Queries.Reviews.GetProductReviews
{
    public class GetProductReviewsQuery : IRequest<ApiResponse<IEnumerable<ReviewDto>>>
    {
        public Guid ProductId { get; set; }

        public GetProductReviewsQuery(Guid productId)
        {
            ProductId = productId;
        }
    }
}
