using Application.Commons.DTOs;
using Application.Commons.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Reviews.CreateReview
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ApiResponse<ReviewDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateReviewCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ReviewDto>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            // 1. Validate Rating
            if (request.Rating < 1 || request.Rating > 5)
            {
                return ApiResponse<ReviewDto>.FailureResult("Điểm đánh giá phải từ 1 đến 5 sao.");
            }

            // 2. Lấy thông tin User để vừa validate vừa dùng cho Mapping
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return ApiResponse<ReviewDto>.FailureResult("Người dùng không tồn tại.");
            }

            // 3. Kiểm tra xem người dùng đã mua sản phẩm này và đơn hàng đã giao thành công (Delivered) chưa
            var userOrders = await _unitOfWork.Orders.GetOrdersByUserIdAsync(request.UserId);
            var hasPurchasedAndDelivered = userOrders.Any(o =>
                o.OrderStatus == OrderStatus.Delivered &&
                o.OrderDetails.Any(od => od.ProductId == request.ProductId));

            if (!hasPurchasedAndDelivered)
            {
                return ApiResponse<ReviewDto>.FailureResult("Bạn chỉ có thể đánh giá sản phẩm sau khi đơn hàng chứa sản phẩm đã được giao thành công.");
            }

            // 4. Kiểm tra xem người dùng đã đánh giá sản phẩm này trước đó chưa
            var existingReview = await _unitOfWork.Reviews.GetReviewByUserAndProductAsync(request.UserId, request.ProductId);
            if (existingReview != null)
            {
                return ApiResponse<ReviewDto>.FailureResult("Bạn đã đánh giá sản phẩm này rồi.");
            }

            // 5. Tạo entity Review mới
            var review = new Review
            {
                ReviewId = Guid.NewGuid(),
                ProductId = request.ProductId,
                UserId = request.UserId,
                User = user, // Gán Entity User vào đây để AutoMapper lấy được FullName
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Reviews.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();

            // 6. Mapping và trả về kết quả
            var reviewDto = _mapper.Map<ReviewDto>(review);

            // Đảm bảo UserName được gán chính xác
            reviewDto.UserName = user.FullName;

            return ApiResponse<ReviewDto>.SuccessResult(reviewDto, "Đánh giá sản phẩm thành công.");
        }
    }
}
