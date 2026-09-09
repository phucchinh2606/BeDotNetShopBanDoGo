using Application.Commons.DTOs;
using Application.Commons.Interfaces;
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
        private readonly IPhotoService _photoService;

        public CreateReviewCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoService = photoService;
        }

        public async Task<ApiResponse<ReviewDto>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            // 1. Validate Rating
            if (request.Rating < 1 || request.Rating > 5)
            {
                return ApiResponse<ReviewDto>.FailureResult("Điểm đánh giá phải từ 1 đến 5 sao.");
            }

            // 2. Kiểm tra User tồn tại
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return ApiResponse<ReviewDto>.FailureResult("Người dùng không tồn tại.");
            }

            // 3. Kiểm tra đơn hàng đã giao (Delivered) chứa sản phẩm này chưa
            var userOrders = await _unitOfWork.Orders.GetOrdersByUserIdAsync(request.UserId);
            var hasPurchasedAndDelivered = userOrders.Any(o =>
                o.OrderStatus == OrderStatus.Delivered &&
                o.OrderDetails.Any(od => od.ProductId == request.ProductId));

            if (!hasPurchasedAndDelivered)
            {
                return ApiResponse<ReviewDto>.FailureResult("Bạn chỉ có thể đánh giá sản phẩm sau khi đơn hàng chứa sản phẩm đã được giao thành công.");
            }

            // 4. Kiểm tra đã đánh giá chưa
            var existingReview = await _unitOfWork.Reviews.GetReviewByUserAndProductAsync(request.UserId, request.ProductId);
            if (existingReview != null)
            {
                return ApiResponse<ReviewDto>.FailureResult("Bạn đã đánh giá sản phẩm này rồi.");
            }

            // 5. Upload hình ảnh lên Cloudinary nếu có đính kèm file
            var imageUrls = new List<string>();
            if (request.Images != null && request.Images.Any())
            {
                try
                {
                    foreach (var file in request.Images)
                    {
                        if (file.Length > 0)
                        {
                            // UploadPhotoAsync trả về trực tiếp chuỗi URL (string)
                            var photoUrl = await _photoService.UploadPhotoAsync(file, "reviews");
                            if (!string.IsNullOrEmpty(photoUrl))
                            {
                                imageUrls.Add(photoUrl);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ApiResponse<ReviewDto>.FailureResult($"Lỗi khi tải ảnh lên Cloudinary: {ex.Message}");
                }
            }

            // 6. Tạo Entity Review
            var review = new Review
            {
                ReviewId = Guid.NewGuid(),
                ProductId = request.ProductId,
                UserId = request.UserId,
                User = user,
                Rating = request.Rating,
                Comment = request.Comment,
                ImageUrls = imageUrls,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Reviews.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();

            // 7. Map kết quả
            var reviewDto = _mapper.Map<ReviewDto>(review);
            reviewDto.UserName = user.FullName;

            return ApiResponse<ReviewDto>.SuccessResult(reviewDto, "Đánh giá sản phẩm thành công.");
        }
    }
}
