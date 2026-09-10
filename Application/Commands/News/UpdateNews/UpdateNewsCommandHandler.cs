using Application.Commons.Exceptions;
using Application.Commons.Interfaces;
using Application.Commons.Models;
using Domain.Interfaces;
using MediatR;
using System.Text;
using System.Text.RegularExpressions;

namespace Application.Commands.News.UpdateNews
{
    public class UpdateNewsCommandHandler : IRequestHandler<UpdateNewsCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;

        public UpdateNewsCommandHandler(IUnitOfWork unitOfWork, IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _photoService = photoService;
        }

        public async Task<ApiResponse<bool>> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
        {
            // 1. Kiểm tra bài viết tin tức có tồn tại không
            var news = await _unitOfWork.News.GetByIdAsync(request.NewsId);
            if (news == null)
            {
                throw new NotFoundException("Bài viết tin tức", request.NewsId);
            }

            // 2. Kiểm tra Tác giả có tồn tại không
            var author = await _unitOfWork.Users.GetByIdAsync(request.AuthorId);
            if (author == null)
            {
                throw new NotFoundException("Tác giả không tồn tại trên hệ thống.");
            }

            // 3. Upload ảnh mới nếu người dùng đính kèm file
            if (request.Image != null && request.Image.Length > 0)
            {
                string newImageUrl = await _photoService.UploadPhotoAsync(request.Image, "news");
                news.ImageUrl = newImageUrl;
            }

            // 4. Cập nhật Slug nếu Tiêu đề thay đổi
            if (!string.Equals(news.Title, request.Title, StringComparison.OrdinalIgnoreCase))
            {
                news.Slug = GenerateSlug(request.Title);
            }

            // 5. Cập nhật các thông tin còn lại
            news.Title = request.Title;
            news.Summary = request.Summary;
            news.Content = request.Content;
            news.Status = request.Status;
            news.AuthorId = request.AuthorId;
            news.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.News.Update(news);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResult(true, "Cập nhật bài viết tin tức thành công.");
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
