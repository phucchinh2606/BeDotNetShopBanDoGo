using Application.Commons.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Categories.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                throw new NotFoundException("Danh mục", request.CategoryId);
            }

            // Kiểm tra xem danh mục này có chứa danh mục con nào không
            var allCategories = await _unitOfWork.Categories.GetAllAsync();
            var hasSubCategories = allCategories.Any(c => c.ParentId == request.CategoryId);
            if (hasSubCategories)
            {
                throw new BadRequestException("Không thể xóa danh mục này vì vẫn còn các danh mục con bên trong.");
            }

            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
