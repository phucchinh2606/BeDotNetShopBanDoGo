using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Categories.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request.ParentId.HasValue)
            {
                var parentCategory = await _unitOfWork.Categories.GetByIdAsync(request.ParentId.Value);
                if (parentCategory == null)
                {
                    throw new Exception("Danh mục cha không tồn tại.");
                }
            }

            // Dùng AutoMapper để chuyển Command thành Entity
            var category = _mapper.Map<Category>(request);

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return category.CategoryId;
        }
    }
}
