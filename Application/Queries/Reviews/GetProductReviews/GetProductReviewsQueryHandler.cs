using Application.Commons.DTOs;
using Application.Commons.Models;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.Reviews.GetProductReviews
{
    public class GetProductReviewsQueryHandler : IRequestHandler<GetProductReviewsQuery, ApiResponse<IEnumerable<ReviewDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductReviewsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<ReviewDto>>> Handle(GetProductReviewsQuery request, CancellationToken cancellationToken)
        {
            // Kiểm tra xem sản phẩm có tồn tại không
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return ApiResponse<IEnumerable<ReviewDto>>.FailureResult("Sản phẩm không tồn tại.");
            }

            // Lấy danh sách Review kèm thông tin User (Đã có Include User và AsNoTracking ở Repository)
            var reviews = await _unitOfWork.Reviews.GetReviewsByProductIdAsync(request.ProductId);

            var reviewDtos = _mapper.Map<IEnumerable<ReviewDto>>(reviews);

            return ApiResponse<IEnumerable<ReviewDto>>.SuccessResult(reviewDtos, "Lấy danh sách đánh giá thành công.");
        }
    }
}
