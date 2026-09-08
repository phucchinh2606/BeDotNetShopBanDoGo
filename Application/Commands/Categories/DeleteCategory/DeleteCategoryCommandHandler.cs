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
                throw new Exception("Không tìm thấy danh mục cần xóa.");
            }

            // Kiểm tra xem danh mục này có chứa danh mục con nào không
            var allCategories = await _unitOfWork.Categories.GetAllAsync();
            var hasSubCategories = allCategories.Any(c => c.ParentId == request.CategoryId);
            if (hasSubCategories)
            {
                throw new Exception("Không thể xóa danh mục này vì vẫn còn các danh mục con bên trong. Vui lòng xóa hoặc di chuyển các danh mục con trước.");
            }

            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
