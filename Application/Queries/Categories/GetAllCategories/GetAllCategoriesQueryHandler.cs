using Application.Commons.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.Categories.GetAllCategories
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var allCategories = await _unitOfWork.Categories.GetAllAsync();

            // Lọc ra các danh mục gốc
            var rootCategories = allCategories.Where(c => c.ParentId == null).ToList();

            // AutoMapper tự động map đệ quy các SubCategories nếu đã khai báo CreateMap<Category, CategoryDto>()
            return _mapper.Map<List<CategoryDto>>(rootCategories);
        }
    }
}
