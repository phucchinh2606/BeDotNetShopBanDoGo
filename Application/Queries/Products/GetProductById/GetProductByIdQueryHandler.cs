using Application.Commons.DTOs;
using Application.Commons.Models;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.Products.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ApiResponse<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetByIdWithCategoryAsync(request.ProductId);

            if (product == null)
            {
                return ApiResponse<ProductDto>.FailureResult("Không tìm thấy sản phẩm.");
            }

            var productDto = _mapper.Map<ProductDto>(product);
            return ApiResponse<ProductDto>.SuccessResult(productDto, "Lấy thông tin sản phẩm thành công.");
        }
    }
}
