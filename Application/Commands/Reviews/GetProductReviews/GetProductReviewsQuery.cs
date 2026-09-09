using Application.Commons.DTOs;
using Application.Commons.Models;
using MediatR;

namespace Application.Commands.Reviews.GetProductReviews
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
