using Application.Commons.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.Categories.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                throw new Exception("Không tìm thấy danh mục với ID này.");
            }

            // Tự động map từ Entity -> Dto bằng 1 dòng duy nhất
            return _mapper.Map<CategoryDto>(category);
        }
    }
}
