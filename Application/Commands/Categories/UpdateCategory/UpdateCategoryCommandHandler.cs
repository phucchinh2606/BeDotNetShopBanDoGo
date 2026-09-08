using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Categories.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                throw new Exception("Không tìm thấy danh mục cần cập nhật.");
            }

            if (request.ParentId.HasValue && request.ParentId.Value == request.CategoryId)
            {
                throw new Exception("Danh mục không thể làm danh mục cha của chính nó.");
            }

            if (request.ParentId.HasValue)
            {
                var parentCategory = await _unitOfWork.Categories.GetByIdAsync(request.ParentId.Value);
                if (parentCategory == null)
                {
                    throw new Exception("Danh mục cha không tồn tại.");
                }
            }

            // Ghi đè thông tin mới từ Command vào Entity thông qua AutoMapper
            _mapper.Map(request, category);

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
