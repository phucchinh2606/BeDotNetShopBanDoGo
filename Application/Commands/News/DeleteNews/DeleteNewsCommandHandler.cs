using Application.Commons.Exceptions;
using Application.Commons.Models;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.News.DeleteNews
{
    public class DeleteNewsCommandHandler : IRequestHandler<DeleteNewsCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteNewsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<bool>> Handle(DeleteNewsCommand request, CancellationToken cancellationToken)
        {
            // 1. Kiểm tra bài viết tin tức có tồn tại không
            var news = await _unitOfWork.News.GetByIdAsync(request.NewsId);
            if (news == null)
            {
                throw new NotFoundException("Bài viết tin tức", request.NewsId);
            }

            // 2. Thực hiện xóa bài viết
            _unitOfWork.News.Delete(news);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResult(true, "Xóa bài viết tin tức thành công.");
        }
    }
}
