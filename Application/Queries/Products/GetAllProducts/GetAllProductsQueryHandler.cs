using Application.Commons.DTOs;
using Application.Commons.Models;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.Products.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, ApiResponse<PagedResult<ProductDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PagedResult<ProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var (products, totalCount) = await _unitOfWork.Products.GetPagedProductsAsync(
                request.SearchTerm,
                request.CategoryId,
                request.MinPrice,
                request.MaxPrice,
                request.Status,
                request.SortBy,
                request.IsDescending,
                request.PageNumber,
                request.PageSize
            );

            var productDtos = _mapper.Map<List<ProductDto>>(products);
            var pagedResult = new PagedResult<ProductDto>(productDtos, totalCount, request.PageNumber, request.PageSize);

            return ApiResponse<PagedResult<ProductDto>>.SuccessResult(pagedResult, "Lấy danh sách sản phẩm thành công.");
        }
    }
}
