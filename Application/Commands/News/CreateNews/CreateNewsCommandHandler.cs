using Application.Commons.Interfaces;
using Application.Commons.Models;
using Domain.Interfaces;
using MediatR;
using System.Text;
using System.Text.RegularExpressions;

namespace Application.Commands.News.CreateNews
{
    public class CreateNewsCommandHandler : IRequestHandler<CreateNewsCommand, ApiResponse<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;

        public CreateNewsCommandHandler(IUnitOfWork unitOfWork, IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _photoService = photoService;
        }

        public async Task<ApiResponse<Guid>> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
        {
            // 1. Kiểm tra tác giả (User) có tồn tại không
            var author = await _unitOfWork.Users.GetByIdAsync(request.AuthorId);
            if (author == null)
            {
                return ApiResponse<Guid>.FailureResult("Tác giả không tồn tại trên hệ thống.");
            }

            // 2. Upload ảnh tin tức lên Cloudinary (nếu có truyền file)
            string imageUrl = string.Empty;
            if (request.Image != null && request.Image.Length > 0)
            {
                imageUrl = await _photoService.UploadPhotoAsync(request.Image, "news");
            }

            // 3. Tạo Slug tự động từ Title
            string slug = GenerateSlug(request.Title);

            // 4. Khởi tạo đối tượng News
            var news = new Domain.Entities.News
            {
                NewsId = Guid.NewGuid(),
                Title = request.Title,
                Slug = slug,
                Summary = request.Summary,
                Content = request.Content,
                ImageUrl = imageUrl,
                Status = request.Status,
                AuthorId = request.AuthorId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.News.AddAsync(news);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<Guid>.SuccessResult(news.NewsId, "Tạo bài viết tin tức thành công.");
        }

        private string GenerateSlug(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return string.Empty;

            string normalizedString = title.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            string result = stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToLower();
            result = Regex.Replace(result, @"[đĐ]", "d");
            result = Regex.Replace(result, @"[^a-z0-9\s-]", "");
            result = Regex.Replace(result, @"\s+", "-").Trim('-');

            return result;
        }
    }
}
